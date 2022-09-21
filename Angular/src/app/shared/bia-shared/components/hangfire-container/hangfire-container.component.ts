import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { Subscription } from "rxjs";
import { AuthService } from "src/app/core/bia-core/services/auth.service";

@Component({
    selector: 'bia-hangfire-container',
    templateUrl: './hangfire-container.component.html'
  })
  export class HangfireContainerComponent implements OnInit, OnDestroy {
    @Input() url: string = "";

    @ViewChild('iFrame2', { static: false }) iFrame2: ElementRef;
    
    private sub = new Subscription();
    public displayFrame = false;
    //public urlToken = ""
    public token = ""

    constructor(private authService: AuthService) {
    }

    ngOnInit() {
      this.sub.add(
        this.authService.getLightToken().subscribe(authinfo => {
          this.token = authinfo.token;
          //this.urlToken = this.url + "?jwt_token=" + this.token;
          let content = `<html><head></head><body style="margin: 0;">
<form id="hangfireForm" target="iFrame1" action="`+ this.url + `" method="POST" style="display:none">
  <input type="text" name="jwt_token" value="` + this.token + `" />
  <input type="submit">
</form>

<iframe  frameborder="0" name="iFrame1" style="width:100%;height:850px;">
    Your browser does not support inline frames.
</iframe>

<script>
    function ready(callback){
      // in case the document is already rendered
      if (document.readyState!='loading') callback();
      // modern browsers
      else if (document.addEventListener) document.addEventListener('DOMContentLoaded', callback);
      // IE <= 8
      else document.attachEvent('onreadystatechange', function(){
          if (document.readyState=='complete') callback();
      });
    }
    ready(function(){
        var hangfireForm= document.getElementById("hangfireForm");
        hangfireForm.submit();
    });
</script>
</html>`

          let doc =  this.iFrame2.nativeElement.contentDocument || this.iFrame2.nativeElement.contentWindow;
          doc.open();
          doc.write(content);
          doc.close();
          this.displayFrame = true;
        })
      );
    }

    ngOnDestroy() {
      // remove cookie HangFireCookie
      document.cookie = 'HangFireCookie=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;'
      if (this.sub) {
        this.sub.unsubscribe();
      }
    }
  }