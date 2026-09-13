import { Injectable, signal, computed } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Router } from "@angular/router";
import { Observable, tap } from "rxjs";
import { environment } from "../../environments/environment";

export interface RegisterRequest {
    email: string;
    password: string;
}

export interface LoginRequest {
    email: string;
    password: string;
}

export interface AuthResponse {
    token: string;
    email: string;
}

const TOKEN_KEY = 'gymtracker_token';
const EMAIL_KEY = 'gymtracker_email';

@Injectable({providedIn: 'root'})
export class AuthService {
    private readonly email = signal<string | null>(localStorage.getItem(EMAIL_KEY));
    readonly isAuthenticated = computed(() => this.email() !== null );
    readonly currentEmail = this.email.asReadonly();

    constructor(private http: HttpClient, private router: Router) {}

    register(request: RegisterRequest): Observable<AuthResponse> {
        return this.http
            .post<AuthResponse>(`${environment.apiUrl}/auth/register`, request)
            .pipe(tap(response => this.setSession(response)));
    }

    login(request: LoginRequest): Observable<AuthResponse> {
    return this.http
        .post<AuthResponse>(`${environment.apiUrl}/auth/login`, request)
        .pipe(tap(response => this.setSession(response)));
    }

    logout(): void {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(EMAIL_KEY);
        this.email.set(null);
        this.router.navigateByUrl('/login');
    }

    getToken(): string | null {
        return localStorage.getItem(TOKEN_KEY);
    }

    private setSession(response: AuthResponse): void {
    localStorage.setItem(TOKEN_KEY, response.token);
    localStorage.setItem(EMAIL_KEY, response.email);
    this.email.set(response.email);
  }


}