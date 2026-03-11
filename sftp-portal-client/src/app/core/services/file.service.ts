import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { FileItem, UploadResult } from '../models/file.model';

@Injectable({
  providedIn: 'root'
})
export class FileService {
  private apiUrl = `${environment.apiUrl}/files`;

  constructor(private http: HttpClient) {}

  // GET api/files?remotePath=...
  listFiles(remotePath: string): Observable<FileItem[]> {
    const params = new HttpParams().set('remotePath', remotePath);
    return this.http.get<FileItem[]>(this.apiUrl, { params });
  }

  // POST api/files/upload?remotePath=...
  uploadFile(remotePath: string, file: File): Observable<HttpEvent<UploadResult>> {
    const params = new HttpParams().set('remotePath', remotePath);
    const formData = new FormData();
    formData.append('file', file);

    return this.http.post<UploadResult>(
      `${this.apiUrl}/upload`,
      formData,
      { params, reportProgress: true, observe: 'events' }
    );
  }

  // GET api/files/download?remotePath=...&fileName=...
  downloadFile(remotePath: string, fileName: string): Observable<Blob> {
    const params = new HttpParams()
      .set('remotePath', remotePath)
      .set('fileName', fileName);

    return this.http.get(`${this.apiUrl}/download`, {
      params,
      responseType: 'blob'
    });
  }
}