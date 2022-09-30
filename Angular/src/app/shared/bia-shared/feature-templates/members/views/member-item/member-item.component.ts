import { Component, Injector, OnInit } from '@angular/core';
import { Member } from '../../model/member';
import { BiaClassicLayoutService } from 'src/app/shared/bia-shared/components/layout/classic-layout/bia-classic-layout.service';
import { first } from 'rxjs/operators';
import { CrudItemItemComponent } from 'src/app/shared/bia-shared/feature-templates/crud-items/views/crud-item-item/crud-item-item.component';
import { MemberService } from '../../services/member.service';

@Component({
  templateUrl: '../../../crud-items/views/crud-item-item/crud-item-item.component.html',
  styleUrls: ['../../../crud-items/views/crud-item-item/crud-item-item.component.scss']
})
export class MemberItemComponent extends CrudItemItemComponent<Member> implements OnInit {
  protected layoutService: BiaClassicLayoutService;
  protected memberService: MemberService;

  constructor(
    protected injector: Injector,
  ) {
    super(injector, injector.get<MemberService>(MemberService));
    this.layoutService = injector.get<BiaClassicLayoutService>(BiaClassicLayoutService);
    this.memberService = injector.get<MemberService>(MemberService);
  }

  ngOnInit() {
    super.ngOnInit();
    this.sub.add
      (
        this.memberService.crudItem$.subscribe((member) => {
          // TODO after creation of CRUD Member : set the field of the item to display in the breadcrump
          if (member?.user?.display) {
            this.route.data.pipe(first()).subscribe(routeData => {
              (routeData as any)['breadcrumb'] = member.user.display;
            });
            this.layoutService.refreshBreadcrumb();
          }
        })
      );
  }
}
