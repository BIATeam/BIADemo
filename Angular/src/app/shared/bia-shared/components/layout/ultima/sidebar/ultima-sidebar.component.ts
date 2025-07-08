import { NgIf, NgTemplateOutlet } from '@angular/common';
import {
  Component,
  ElementRef,
  Input,
  OnDestroy,
  ViewChild,
} from '@angular/core';
import { RouterLink } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { MenuItem } from 'primeng/api';
import { Subscription } from 'rxjs';
import { BiaTranslationService } from 'src/app/core/bia-core/services/bia-translation.service';
import { BiaNavigation } from 'src/app/shared/bia-shared/model/bia-navigation';
import { allEnvironments } from 'src/environments/all-environments';
import { BiaLayoutService } from '../../services/layout.service';
import { BiaUltimaMenuProfileComponent } from '../menu-profile/ultima-menu-profile.component';
import { BiaUltimaMenuComponent } from '../menu/ultima-menu.component';

@Component({
  selector: 'bia-ultima-sidebar',
  templateUrl: './ultima-sidebar.component.html',
  styleUrls: ['./ultima-sidebar.component.scss'],
  imports: [
    RouterLink,
    BiaUltimaMenuProfileComponent,
    BiaUltimaMenuComponent,
    NgIf,
    NgTemplateOutlet,
  ],
})
export class BiaUltimaSidebarComponent implements OnDestroy {
  timeout: any = null;
  @Input() appTitle: string;
  @Input() version: string;
  @Input() username: string | undefined;
  @Input() lastname: string | undefined;
  @Input() login: string;
  @Input()
  set menus(navigations: BiaNavigation[]) {
    this.navigations = navigations ?? [];
    this.buildNavigation();
  }
  @Input() allowThemeChange = true;
  @Input() logos: string[];
  @Input() helpUrl?: string;
  @Input() reportUrl?: string;

  @ViewChild(BiaUltimaMenuProfileComponent)
  menuProfile!: BiaUltimaMenuProfileComponent;
  @ViewChild('menuContainer') menuContainer!: ElementRef;
  navigations: BiaNavigation[];
  navMenuItems: MenuItem[];
  urlAppIcon = allEnvironments.urlAppIcon;

  protected sub = new Subscription();

  constructor(
    public layoutService: BiaLayoutService,
    public biaTranslationService: BiaTranslationService,
    public translateService: TranslateService,
    public el: ElementRef
  ) {}

  buildNavigation() {
    const translationKeys = new Array<string>();
    this.navigations.forEach(menu => {
      if (menu.children) {
        menu.children.forEach(child => {
          translationKeys.push(child.labelKey);
        });
      }
      translationKeys.push(menu.labelKey);
    });

    this.navMenuItems = this.layoutService.mapNavigationToMenuItems(
      this.navigations,
      true
    );

    if (translationKeys.length > 0) {
      this.sub.add(
        this.translateService
          .stream(translationKeys)
          .subscribe(translations => {
            this.layoutService.processMenuTranslation(
              this.navMenuItems,
              translations
            );
          })
      );
    }
  }

  resetOverlay() {
    if (this.layoutService.state.overlayMenuActive) {
      this.layoutService.state.overlayMenuActive = false;
    }
  }

  onMouseEnter() {
    if (!this.layoutService.state.anchored) {
      if (this.timeout) {
        clearTimeout(this.timeout);
        this.timeout = null;
      }
      this.layoutService.state.sidebarActive = true;
    }
  }

  onMouseLeave() {
    if (!this.layoutService.state.anchored) {
      if (!this.timeout) {
        this.timeout = setTimeout(
          () => (this.layoutService.state.sidebarActive = false),
          300
        );
      }
    }
  }

  anchor() {
    this.layoutService.state.anchored = !this.layoutService.state.anchored;
  }

  ngOnDestroy() {
    this.resetOverlay();
  }
}
