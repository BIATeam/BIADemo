import { Component, HostBinding, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { getAllMembers, getMembersTotalCount, getMemberLoadingGetAll } from '../../store/member.state';
import { multiRemove, loadAllByPost, update, create } from '../../store/members-actions';
import { Observable } from 'rxjs';
import { LazyLoadEvent } from 'primeng/api';
import { Member } from '../../model/member';
import { BiaTableComponent } from 'src/app/shared/bia-shared/components/table/bia-table/bia-table.component';
import {
  BiaListConfig,
  PrimeTableColumn,
  PropType
} from 'src/app/shared/bia-shared/components/table/bia-table/bia-table-config';
import { AppState } from 'src/app/store/state';
import { DEFAULT_PAGE_SIZE } from 'src/app/shared/constants';
import { AuthService } from 'src/app/core/bia-core/services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { MemberDas } from '../../services/member-das.service';
import * as FileSaver from 'file-saver';
import { TranslateService } from '@ngx-translate/core';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { Permission } from 'src/app/shared/permission';
import { KeyValuePair } from 'src/app/shared/bia-shared/model/key-value-pair';
import { MembersSignalRService } from '../../services/member-signalr.service';
import { MembersEffects } from '../../store/members-effects';
import { loadAllView } from 'src/app/shared/bia-shared/features/view/store/views-actions';
import { MemberOptionsService } from '../../services/member-options.service';
import { SiteService } from 'src/app/features/sites/services/site.service';

@Component({
  selector: 'app-members-index',
  templateUrl: './members-index.component.html',
  styleUrls: ['./members-index.component.scss']
})
export class MembersIndexComponent implements OnInit, OnDestroy {
  useCalcMode = false;
  useSignalR = true;
  useView = false;

  @HostBinding('class.bia-flex') flex = true;
  @ViewChild(BiaTableComponent, { static: false }) memberListComponent: BiaTableComponent;
  showColSearch = false;
  globalSearchValue = '';
  defaultPageSize = DEFAULT_PAGE_SIZE;
  pageSize = this.defaultPageSize;
  totalRecords: number;
  members$: Observable<Member[]>;
  selectedMembers: Member[];
  totalCount$: Observable<number>;
  loading$: Observable<boolean>;
  canEdit = false;
  canDelete = false;
  canAdd = false;
  tableConfiguration: BiaListConfig;
  columns: KeyValuePair[];
  displayedColumns: KeyValuePair[];
  viewPreference: string;
  popupTitle: string;
  tableStateKey = this.useView ? 'membersGrid' : undefined;


  constructor(
    private store: Store<AppState>,
    private router: Router,
    public activatedRoute: ActivatedRoute,
    private authService: AuthService,
    private memberDas: MemberDas,
    private translateService: TranslateService,
    private biaTranslationService: BiaTranslationService,
    private membersSignalRService: MembersSignalRService,
    public memberOptionsService: MemberOptionsService,
    public siteService: SiteService,
  ) {
  }

  ngOnInit() {
    this.initTableConfiguration();
    this.setPermissions();
    this.members$ = this.store.select(getAllMembers);
    this.totalCount$ = this.store.select(getMembersTotalCount);
    this.loading$ = this.store.select(getMemberLoadingGetAll);
    this.OnDisplay();
    if (this.useCalcMode) {
      this.memberOptionsService.loadAllOptions();
    }
  }

  ngOnDestroy() {
    this.OnHide();
  }

  OnDisplay() {
    if (this.memberListComponent !== undefined) {
      this.store.dispatch(loadAllByPost({ event: this.memberListComponent.getLazyLoadMetadata() }));
    }

    if (this.useView)
    {
      this.store.dispatch(loadAllView());
    }


    if (this.useSignalR)
    {
      this.membersSignalRService.initialize();
      MembersEffects.useSignalR = true;
    }
  }

  OnHide() {
    if (this.useSignalR)
    {
      MembersEffects.useSignalR = false;
      this.membersSignalRService.destroy();
    }
  }

  onCreate() {
    if (!this.useCalcMode) {
      this.router.navigate(['../create'], { relativeTo: this.activatedRoute });
    }
  }

  onEdit(memberId: number) {
    if (!this.useCalcMode) {
      this.router.navigate(['../' + memberId + '/edit'], { relativeTo: this.activatedRoute });
    }
  }

  onSave(member: Member) {
    if (this.useCalcMode) {
      if (member?.id > 0) {
        if (this.canEdit) {
          this.store.dispatch(update({ member: member }));
        }
      } else {
        if (this.canAdd) {
          this.store.dispatch(create({ member: member }));
        }
      }
    }
  }

  onDelete() {
    if (this.selectedMembers && this.canDelete) {
      this.store.dispatch(multiRemove({ ids: this.selectedMembers.map((x) => x.id) }));
    }
  }

  onSelectedElementsChanged(members: Member[]) {
    this.selectedMembers = members;
  }

  onPageSizeChange(pageSize: number) {
    this.pageSize = pageSize;
  }

  /*onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    this.store.dispatch(loadAllByPost({ event: lazyLoadEvent }));
  }*/
  onLoadLazy(lazyLoadEvent: LazyLoadEvent) {
    if (this.siteService.currentSiteId > 0) {
      const customEvent: any = { siteId: + this.siteService.currentSiteId, ...lazyLoadEvent };
      this.store.dispatch(loadAllByPost({ event: customEvent }));
    }
  }


  searchGlobalChanged(value: string) {
    this.globalSearchValue = value;
  }

  displayedColumnsChanged(values: KeyValuePair[]) {
    this.displayedColumns = values;
  }

  onToggleSearch() {
    this.showColSearch = !this.showColSearch;
  }

  onViewChange(viewPreference: string) {
    this.viewPreference = viewPreference;
  }

  onExportCSV() {
    const columns: { [key: string]: string } = {};
    this.columns.map((x) => (columns[x.value.split('.')[1]] = this.translateService.instant(x.value)));
    const customEvent: any = { columns: columns, ...this.memberListComponent.getLazyLoadMetadata() };
    this.memberDas.getFile(customEvent).subscribe((data) => {
      FileSaver.saveAs(data, this.translateService.instant('app.members') + '.csv');
    });
  }

  private setPermissions() {
    this.canEdit = this.authService.hasPermission(Permission.Member_Update);
    this.canDelete = this.authService.hasPermission(Permission.Member_Delete);
    this.canAdd = this.authService.hasPermission(Permission.Member_Create);
  }

  private initTableConfiguration() {
    this.biaTranslationService.culture$.subscribe((dateFormat) => {
      this.tableConfiguration = {
        columns: [
          Object.assign(new PrimeTableColumn('user', 'member.user'), {
            type: PropType.OneToMany
          }),
          Object.assign(new PrimeTableColumn('roles', 'member.rolesForSite'), {
            type: PropType.ManyToMany,
            translateKey: 'role.',
            searchPlaceholder: 'Site_Admin|Pilot|...'
          })
        ]
      };

      this.columns = this.tableConfiguration.columns.map((col) => <KeyValuePair>{ key: col.field, value: col.header });
      this.displayedColumns = [...this.columns];
    });
  }
}
