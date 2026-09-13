import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

export type ExerciseType = 'Cardio' | 'Strength' | 'Flexibility';

export interface WorkoutRequest {
    type: ExerciseType;
    performedAt: string;
    durationMinutes: number;
    calories: number;
    intensity: number;
    fatigue: number;
    notes?: string;
}

export interface WorkoutDto extends WorkoutRequest {
  id: string;
}

@Injectable({ providedIn: 'root' })
export class WorkoutService {
  private readonly baseUrl = `${environment.apiUrl}/workouts`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<WorkoutDto[]> {
    return this.http.get<WorkoutDto[]>(this.baseUrl);
  }

  create(request: WorkoutRequest): Observable<WorkoutDto> {
    return this.http.post<WorkoutDto>(this.baseUrl, request);
  }

  delete(id: string): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}