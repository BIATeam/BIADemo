import { NgModule } from '@angular/core';
import { EffectsModule } from '@ngrx/effects';
import { StoreModule } from '@ngrx/store';
import { reducers } from './store/site.state';
import { SitesEffects } from './store/sites-effects';


@NgModule({
  imports: [
    StoreModule.forFeature('domain-sites', reducers),
    EffectsModule.forFeature([SitesEffects]),
  ],
  providers: []
})
export class SiteModule {}


















