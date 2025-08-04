import { BiaLayoutService } from '../components/layout/services/layout.service';

export class LayoutHelperService {
  // Default height of container when subtracting default layout
  public static defaultContainerHeight(
    layoutService: BiaLayoutService,
    offset?: string
  ): string {
    let height: string;
    // topbar = 4rem
    // breadcrumb = 2.5rem
    // padding page = 2rem
    // bia-page-margin : 1.5rem

    if (layoutService.state.fullscreen || layoutService.state.isInIframe) {
      height = '100vh - 3.5rem';
    } else {
      if (layoutService.isBreadcrumbVisible) {
        height = '100vh - 10rem';
      } else {
        height = '100vh - 7.5rem';
      }

      height +=
        layoutService._config.footerMode !== 'overlay'
          ? ' - var(--footer-height)'
          : '';
    }
    if (
      layoutService._config.menuMode === 'horizontal' &&
      !layoutService.state.isSmallScreen
    ) {
      height += ' - 3rem';
    }

    if (offset) {
      height += ` ${offset}`;
    }
    return height;
  }
}
