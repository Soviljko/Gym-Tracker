import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export interface WeeklySummary {
    weekStart: string;
    weekEnd: string;
    workoutCount: number;
    totalDurationMinutes: number;
    averageIntensity: number;
    averageFatigue: number;
}

@Injectable({ providedIn: 'root' })
export class ProgressService {
    private readonly baseUrl = `${environment.apiUrl}/progress`;

    constructor(private http: HttpClient) {}

    getMonthlyProgress(year: number, month: number): Observable<WeeklySummary[]> {
        return this.http.get<WeeklySummary[]>(this.baseUrl, {
            params: { year, month}
        });
    }
}