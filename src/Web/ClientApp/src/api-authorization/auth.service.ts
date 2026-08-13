import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { tap, catchError, map } from 'rxjs/operators';
import { LoginCommand, RegisterCommand, UsersClient } from '../app/web-api-client';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly tokenKey = 'auth_token';

  private _isAuthenticated = new BehaviorSubject<boolean>(false);
  isAuthenticated$ = this._isAuthenticated.asObservable();

  constructor(private usersClient: UsersClient) { }

  /** Call on app startup to check if a stored token is still valid. */
  initialize(): Observable<boolean> {
    if (!this.getToken()) {
      this._isAuthenticated.next(false);
      return of(false);
    }

    return this.usersClient.getMe().pipe(
      map(() => true),
      catchError(() => {
        this.clearToken();
        return of(false);
      }),
      tap(isAuth => this._isAuthenticated.next(isAuth))
    );
  }

  login(email: string, password: string): Observable<void> {
    return this.usersClient.login(new LoginCommand({ userNameOrEmail: email, password })).pipe(
      tap(res => {
        if (res.token) {
          this.setToken(res.token);
        }
        this._isAuthenticated.next(true);
      }),
      map(() => void 0)
    );
  }

  register(email: string, password: string, userName?: string): Observable<void> {
    return this.usersClient.register(new RegisterCommand({ userName: userName ?? email, email, password })).pipe(
      tap(res => {
        if (res.token) {
          this.setToken(res.token);
          this._isAuthenticated.next(true);
        }
      }),
      map(() => void 0)
    );
  }

  /** No server-side logout endpoint exists anymore; auth is a stateless JWT. */
  logout(): Observable<void> {
    this.clearToken();
    this._isAuthenticated.next(false);
    return of(void 0);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  private setToken(token: string): void {
    localStorage.setItem(this.tokenKey, token);
  }

  private clearToken(): void {
    localStorage.removeItem(this.tokenKey);
  }
}