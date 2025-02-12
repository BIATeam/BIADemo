import { CommonModule } from '@angular/common';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { DropdownModule } from 'primeng/dropdown';
import { MultiSelectModule } from 'primeng/multiselect';
import { TooltipModule } from 'primeng/tooltip';
import { BiaTeamSelectorComponent } from './bia-team-selector.component';

@NgModule({ declarations: [BiaTeamSelectorComponent],
    exports: [BiaTeamSelectorComponent], imports: [CommonModule,
        FormsModule,
        DropdownModule,
        MultiSelectModule,
        TooltipModule,
        TranslateModule], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class BiaTeamSelectorModule {}
