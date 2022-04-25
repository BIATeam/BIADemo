import { createAction, props } from '@ngrx/store';
import { OptionDto } from 'src/app/shared/bia-shared/model/option-dto';

export namespace DomainLanguageOptionsActions
{ 
    export const loadAll = createAction('[Domain Language Options] Load all');

    export const loadAllSuccess = createAction('[Domain Language Options] Load all success', props<{ languages: OptionDto[] }>());
    /*
    export const load = createAction('[Domain Languages] Load', props<{ id: number }>());

    export const loadSuccess = createAction('[Domain Languages] Load success', props<{ language: LanguageOption }>());
    */
    export const failure = createAction('[Domain Language Options] Failure', props<{ error: any }>());
}

















