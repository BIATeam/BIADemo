import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { Subscription } from "rxjs";
import { AuthService } from "src/app/core/bia-core/services/auth.service";

@Component({
    selector: 'bia-hangfire-container',
    templateUrl: './hangfire-container.component.html'
  })
  export class HangfireContainerComponent implements OnInit, OnDestroy {
    @Input() url: string = "";

    @ViewChild('iframe') iframe: ElementRef;
    
    private sub = new Subscription();
    public displayFrame = false;
    // public urlToken = ""

    constructor(private authService: AuthService) {
      // Set cookie HangFireCookie
    }

    ngOnInit() {
      this.sub.add(
        this.authService.getLightToken().subscribe(authinfo => {
          let token : string = authinfo.token;
          //this.urlToken = this.url + "?jwt_token=" + token;
          this.displayFrame = true;
          this.getSrc(token);
        })
      );
    }

    async getSrc(token: string) {
      const res = await fetch(this.url, {
        method: 'GET',
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      const blob = await res.blob();
      const urlObject = URL.createObjectURL(blob);
      this.iframe.nativeElement.src = urlObject;
    }

    ngOnDestroy() {
      // remove cookie HangFireCookie
      document.cookie = 'HangFireCookie=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;'
      if (this.sub) {
        this.sub.unsubscribe();
      }
    }
  }