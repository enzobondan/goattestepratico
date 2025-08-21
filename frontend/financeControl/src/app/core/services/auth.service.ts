import { inject, Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, BehaviorSubject, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';
import { ApiService } from './api.service';
import { LoginRequest, LoginResponse, User, Tenant } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  constructor(private api: ApiService, private router: Router) {}

  login(credentials: LoginRequest): Observable<LoginResponse> {
    return this.api.post<LoginResponse>('auth/login', credentials).pipe(
      tap((response) => {
        this.api.setToken(response.token);
      })
    );
  }

  logout(): void {
    this.api.logout();
    this.router.navigate(['/login']);
  }

  isAuthenticated(): boolean {
    return !!this.api.getToken();
  }

  getCurrentUser(): Observable<User> {
    return this.api.get<User>('Auth/current-user');
  }
}
