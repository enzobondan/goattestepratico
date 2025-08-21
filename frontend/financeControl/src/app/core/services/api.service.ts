import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { CONFIG } from '../../app.config';

@Injectable({
  providedIn: 'root',
})
export class ApiService {
  private http = inject(HttpClient);
  private apiUrl = CONFIG.API_URL;
  private authToken: string | null = null;

  constructor() {
    this.authToken = localStorage.getItem('auth_token');
  }

  private handleError(error: any) {
    let errorMessage = 'Ocorreu um erro inesperado';

    if (error.error instanceof ErrorEvent) {
      errorMessage = `Erro: ${error.error.message}`;
    } else {
      errorMessage = error.error?.message || error.message;

      if (error.status === 401) {
        errorMessage = 'Sessão expirada. Faça login novamente.';
      } else if (error.status === 403) {
        errorMessage = 'Acesso não autorizado.';
      } else if (error.status === 404) {
        errorMessage = 'Recurso não encontrado.';
      }
    }

    return throwError(() => new Error(errorMessage));
  }

  setToken(token: string): void {
    localStorage.setItem('auth_token', token);
    this.authToken = token;
  }

  getToken(): string | null {
    return this.authToken;
  }

  logout(): void {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('');
    this.authToken = null;
  }
  get<T>(endpoint: string, params?: any): Observable<T> {
    const httpParams = new HttpParams({ fromObject: params });
    return this.http
      .get<T>(`${this.apiUrl}/${endpoint}`, { params: httpParams })
      .pipe(catchError(this.handleError));
  }

  post<T>(endpoint: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${endpoint}`, data).pipe(catchError(this.handleError));
  }

  put<T>(endpoint: string, data: any): Observable<T> {
    return this.http.put<T>(`${this.apiUrl}/${endpoint}`, data).pipe(catchError(this.handleError));
  }

  patch<T>(endpoint: string, data: any): Observable<T> {
    return this.http
      .patch<T>(`${this.apiUrl}/${endpoint}`, data)
      .pipe(catchError(this.handleError));
  }

  delete<T>(endpoint: string): Observable<T> {
    return this.http.delete<T>(`${this.apiUrl}/${endpoint}`).pipe(catchError(this.handleError));
  }

  upload<T>(endpoint: string, formData: FormData): Observable<T> {
    return this.http
      .post<T>(`${this.apiUrl}/${endpoint}`, formData)
      .pipe(catchError(this.handleError));
  }
}
