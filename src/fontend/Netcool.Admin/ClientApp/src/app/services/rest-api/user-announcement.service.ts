import { CrudRestServiceBase } from "./crud-rest.service";
import { UserAnnouncement, UserAnnouncementReadInput } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable, Subject } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class UserAnnouncementService extends CrudRestServiceBase<UserAnnouncement> {

  read$ = new Subject<number>();

  constructor(protected http: HttpClient) {
    super("api/user-announcements", http);
  }

  read(input: UserAnnouncementReadInput): Observable<any> {
    return this.http.post(`${this.apiPrefix}/read`, input);
  }
}
