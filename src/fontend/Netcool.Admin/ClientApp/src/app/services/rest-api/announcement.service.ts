import { CrudRestServiceBase } from "./crud-rest.service";
import { Announcement } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root"
})
export class AnnouncementService extends CrudRestServiceBase<Announcement> {
  constructor(protected http: HttpClient) {
    super("api/announcements", http);
  }

  publish(id: number): Observable<any> {
    return this.http.put(`${this.apiPrefix}/${id}/publish`, {})
  }

}
