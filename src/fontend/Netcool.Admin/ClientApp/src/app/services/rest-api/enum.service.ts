import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { NameValue } from "@models";
import { tap } from "rxjs/operators";

@Injectable({
  providedIn: 'root'
})
export class EnumService {

  private loading: boolean = false;
  private enums: Map<string, NameValue<number>[]>;

  constructor(private http: HttpClient) {
  }

  public getEnum(key: string): NameValue<number>[] {
    if (this.enums == null) return [];
    return this.enums.get(key.trim().toLowerCase());
  }

  public loadEnums(): void {
    this.loading = true;
    this.http.get('api/enums/items').pipe(tap(() => {
      this.loading = false;
    })).subscribe(data => {
      this.enums = new Map();
      if (data) {
        for (let key in data) {
          if (!data.hasOwnProperty(key)) {
            continue;
          }
          this.enums.set(key, data[key]);
        }
      }
    })
  }

}
