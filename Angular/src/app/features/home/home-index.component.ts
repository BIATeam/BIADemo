import { Component, OnDestroy, OnInit } from '@angular/core';
import { BiaLayoutService } from 'src/app/shared/bia-shared/components/layout/services/layout.service';

@Component({
  selector: 'app-home-index',
  templateUrl: './home-index.component.html',
  styleUrls: ['./home-index.component.scss'],
})
export class HomeIndexComponent implements OnInit, OnDestroy {
  constructor(private layoutService: BiaLayoutService) {}
  ngOnInit(): void {
    this.layoutService.hideBreadcrumb();
  }
  ngOnDestroy(): void {
    this.layoutService.showBreadcrumb();
  }
}
