import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd/modal';
import { AnnouncementService, NotificationService, UserAnnouncementService } from "@services";
import { switchMap } from "rxjs/operators";
import { SettingsService } from "@delon/theme";
import { of } from "rxjs";
import { debuglog } from "util";

@Component({
  selector: 'sys-announcement-view',
  templateUrl: './view.component.html',
  styleUrls:['./view.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class SysAnnouncementViewComponent implements OnInit {
  id: any = {};
  isRead: false;
  i: any;

  constructor(
    private modal: NzModalRef,
    private notificationService: NotificationService,
    private apiService: AnnouncementService,
    private uaService: UserAnnouncementService,
    private settingsService: SettingsService) {
  }

  ngOnInit(): void {
    console.log(this.id)
    this.apiService.get(this.id)
      .pipe(switchMap((announcement) => {
        this.i = announcement;
        if (this.isRead) return of();
        let input = {userId: this.settingsService.user.id, announcementIds: [this.id]};
        return this.uaService.read(input);
      }))
      .subscribe(() => {
        this.uaService.read$.next(this.id);
      })
  }

  close(): void {
    this.modal.destroy();
  }
}
