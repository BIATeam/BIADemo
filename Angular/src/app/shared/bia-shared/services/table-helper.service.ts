import { Injectable } from '@angular/core';
import { FilterMetadata } from 'primeng/api';
import { TABLE_FILTER_GLOBAL } from '../../constants';
import { BiaTableComponent } from '../components/table/bia-table/bia-table.component';

@Injectable({
    providedIn: 'root'
})
export class TableHelperService  {

    constructor(  ) {

    }
    
    public hasFilter(biaTableComponent: BiaTableComponent, ignoreGlobalFilter : boolean) : boolean
    {
      if (this.isNullUndefEmptyStr(biaTableComponent))
      {
        return false;
      }
      if (biaTableComponent.table.hasFilter())
      {
        if (this.isNullUndefEmptyFilters(biaTableComponent.table.filters, true))
        {
          return false;
        }
        else 
        {
          return true;;
        }
      }
      else
      {
        return false;
      }
    }

  
    public isNullUndefEmptyFilters(filters : {[s: string]: FilterMetadata | FilterMetadata[]| undefined;}, ignoreGlobalFilter : boolean) : boolean
    {
      if (!filters) 
      {
        return true;
      }
      if (this.isNullUndefEmptyStr(filters )) 
      {
        return true;
      }
      if (JSON.stringify(filters) === "{}")
      {
        return true;
      }
      for (const [key, filter] of Object.entries(filters)) {
        if (ignoreGlobalFilter && key.startsWith(TABLE_FILTER_GLOBAL))
        {
          continue;
        }
        if (this.isNullUndefEmptyStr(filter)) 
        {
          continue;
        }
        if ((<FilterMetadata>filter).value === null || (<FilterMetadata>filter).value === '' )
        {
          continue;
        }
        if ((<FilterMetadata>filter).value === undefined) 
        {
          // filter is probably a FilterMetadata[]
          for (const filter2 of (filter as FilterMetadata[])) {
            if (this.isNullUndefEmptyStr(filter2)) 
            {
              continue;
            }
            if (this.isNullUndefEmptyStr(filter2.value)) 
            {
              continue;
            }
            return false;
          }
          continue;
        }
        return false;
      }
      return true;
    }
    
    private isNullUndefEmptyStr(obj : any) : boolean
    {
      return (obj === null || obj === undefined || obj === '') 
    }
    
}
