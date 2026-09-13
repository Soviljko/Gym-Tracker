import { Component, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { ProgressService, WeeklySummary } from '../../services/progress.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-progress',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './progress.component.html',
  styleUrl: './progress.component.scss'
})
export class ProgressComponent implements OnInit {
  summaries = signal<WeeklySummary[]>([]);
  errorMessage = signal('');
  selectedMonth: string;

  constructor(
    private progressService: ProgressService,
    private authService: AuthService
  ) {
    const now = new Date();
    this.selectedMonth = `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, '0')}`;
  }

  ngOnInit(): void {
    this.loadProgress();
  }

  onMonthChange(): void {
    this.loadProgress();
  }

  loadProgress(): void {
    const [year, month] = this.selectedMonth.split('-').map(Number);

    this.progressService.getMonthlyProgress(year, month).subscribe({
      next: (data) => this.summaries.set(data),
      error: () => this.errorMessage.set('Error loading progress.')
    });
  }

  logout(): void {
    this.authService.logout();
  }
}
