import { CrudRestServiceBase } from "./crud-rest.service";
import { AppConfig,  } from "@models";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable({
  providedIn: "root"
})
export class AppConfigService extends CrudRestServiceBase<AppConfig> {
  constructor(protected http: HttpClient) {
    super("api/app-configurations", http);
  }

}
