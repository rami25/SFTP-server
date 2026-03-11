import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { FolderItem } from '../models/folder.model';

@Injectable({
  providedIn: 'root'
})
export class FolderService {
  private apiUrl = `${environment.apiUrl}/folders`;

  constructor(private http: HttpClient) {}

  // GET api/folders/{entity}
  getFolders(entity: string): Observable<FolderItem[]> {
    return this.http.get<FolderItem[]>(`${this.apiUrl}/${entity}`);
  }
}