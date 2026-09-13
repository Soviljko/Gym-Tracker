import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { WorkoutDto, WorkoutService } from '../../services/workout.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-workouts',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule, RouterLink],
  templateUrl: './workouts.component.html',
  styleUrl: './workouts.component.scss'
})
export class WorkoutsComponent implements OnInit {
  workouts = signal<WorkoutDto[]>([]);
  errorMessage = signal('');
  isSubmitting = signal(false);
  form: FormGroup;

  constructor(
    private workoutService: WorkoutService,
    private authService: AuthService,
    private fb: FormBuilder
  ) {
    this.form = this.fb.group({
      type: ['Cardio', Validators.required],
      performedAt: ['', Validators.required],
      durationMinutes: [30, [Validators.required, Validators.min(1)]],
      calories: [100, [Validators.required, Validators.min(0)]],
      intensity: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
      fatigue: [5, [Validators.required, Validators.min(1), Validators.max(10)]],
      notes: ['']
    });
  }

  ngOnInit(): void {
    this.loadWorkouts();
  }

  loadWorkouts(): void {
    this.workoutService.getAll().subscribe({
      next: (data) => this.workouts.set(data),
      error: () => this.errorMessage.set('Error loading workouts.')
    });
  }

  onSubmit(): void {
    if (this.form.invalid || this.isSubmitting()) {
      return;
    }

    const value = this.form.value;
    const request = {
      ...value,
      performedAt: new Date(value.performedAt).toISOString()
    };

    this.isSubmitting.set(true);

    this.workoutService.create(request).subscribe({
      next: () => {
        this.form.reset({ type: 'Cardio', durationMinutes: 30, calories: 100, intensity: 5, fatigue: 5, notes: '' });
        this.isSubmitting.set(false);
        this.loadWorkouts();
      },
      error: () => {
        this.errorMessage.set('Error adding workout.');
        this.isSubmitting.set(false);
      }
    });
  }

  onDelete(id: string): void {
    this.workoutService.delete(id).subscribe({
      next: () => this.loadWorkouts(),
      error: () => this.errorMessage.set('Error deleting workout.')
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
