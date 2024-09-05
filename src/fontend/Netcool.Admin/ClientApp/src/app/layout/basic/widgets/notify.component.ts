import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { NzI18nService } from 'ng-zorro-antd/i18n';
import { UserAnnouncementService } from "@services";
import { format } from "date-fns";
import { DrawerHelper, ModalHelper, SettingsService } from "@delon/theme";
import { NoticeIconList, NoticeIconModule, NoticeIconSelect, NoticeItem } from '@delon/abc/notice-icon';
import { SysAnnouncementViewComponent } from "../../../routes/sys/announcement/view/view.component";
import { SysUserAnnouncementComponent } from "../../../routes/sys/announcement/user-announcement/user-announcement.component";

@Component({
  selector: 'header-notify',
  template: `
    <notice-icon
      [data]="data"
      [count]="count"
      [loading]="loading"
      btnClass="alain-default__nav-item"
      btnIconClass="alain-default__nav-item-icon"
      (select)="select($event)"
      (clear)="clear($event)"
    ></notice-icon>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
  standalone: true,
  imports: [NoticeIconModule]
})
export class HeaderNotifyComponent implements OnInit, OnDestroy {
  data: NoticeItem[] = [
    {
      title: '公告',
      list: [],
      emptyText: '你已查看所有公告',
      //emptyImage: 'https://gw.alipayobjects.com/zos/rmsportal/wAhyIChODzsoKIOBHcBk.svg',
      clearText: '查看所有',
    },
    {
      title: '消息',
      list: [],
      emptyText: '您已读完所有消息',
      //emptyImage: 'https://gw.alipayobjects.com/zos/rmsportal/sAuJeJzSKbUmHfBQRzmZ.svg',
      clearText: '清空消息',
    }
  ];
  count = 0;
  loading = false;
  userId: number;

  constructor(private nzI18n: NzI18nService,
              private modal: ModalHelper,
              private drawer: DrawerHelper,
              private settingsService: SettingsService,
              private cdr: ChangeDetectorRef,
              private userAnnouncementService: UserAnnouncementService) {
  }

  ngOnDestroy(): void {
    this.userAnnouncementService.read$.unsubscribe();
  }

  ngOnInit() {
    this.userId = this.settingsService.user.id;
    this.loadData();
    this.userAnnouncementService.read$.subscribe(id => {
      let list = this.data.find(n => n.title == '公告').list;
      let index = list.findIndex(t => t.id == id);
      if (index !== -1) {
        list.splice(index, 1);
        this.data = this.updateNoticeData(list);
        this.count--;
        this.cdr.detectChanges();
      }
      console.log(this.data.find(n => n.title == '公告').list);
    })
  }

  private updateNoticeData(notices: NoticeIconList[]): NoticeItem[] {
    const data = this.data.slice();
    data.forEach((i) => (i.list = []));

    notices.forEach((item) => {
      const newItem = {...item} as NoticeIconList;

      if (newItem.updateTime) {
        newItem.datetime = format(newItem.updateTime, 'yyyy-MM-dd HH:mm');
      }
      if (newItem.extra && newItem.status) {
        newItem.color = ({
          todo: undefined,
          processing: 'blue',
          urgent: 'red',
          doing: 'gold',
        } as { [key: string]: string | undefined })[newItem.status];
      }
      data.find((w) => w.title === newItem.type)!.list.push(newItem);
    });
    return data;
  }

  loadData(): void {
    if (this.loading) {
      return;
    }
    this.loading = true;
    let notifyList = [];
    this.userAnnouncementService.page(1, 50, {userId: this.userId, isRead: false})
      .subscribe((data) => {
        data.items.forEach(item => {
          notifyList.push({
            id: item.announcement.id,
            title: item.announcement.title,
            datetime: item.announcement.updateTime,
            updateTime: item.announcement.updateTime,
            type: '公告',
            isRead: item.isRead
          })
        })
        this.count = data.items.length;
        this.data = this.updateNoticeData(notifyList);
        //this.cdr.detectChanges();
        this.loading = false;
      });
  }

  clear(type: string): void {
    this.drawer.create('公告列表', SysUserAnnouncementComponent, {}, {
      drawerOptions: {
        nzMaskClosable: false, nzWidth: 600
      }
    }).subscribe();
  }

  select(res: NoticeIconSelect): void {
    let id = res.item['id'];
    let isRead = res.item['isRead'];
    //let type = res.item['type'];

    this.modal
      .create(SysAnnouncementViewComponent, {id: id, isRead: isRead}, {
        modalOptions: {
          nzMaskClosable: false,
          nzKeyboard: false
        }
      })
      .subscribe(() => {
      });
  }
}
