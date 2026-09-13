import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
    { path: 'login', loadComponent: () => import('./pages/login/login.component').then(m => m.LoginComponent) },
    { path: 'register', loadComponent: () => import('./pages/register/register.component').then(m => m.RegisterComponent) },
    {
        path: 'workouts',
        loadComponent: () => import('./pages/workouts/workouts.component').then(m => m.WorkoutsComponent),
        canActivate: [authGuard]
    },
    {
        path: 'progress',
        loadComponent: () => import('./pages/progress/progress.component').then(m => m.ProgressComponent),
        canActivate: [authGuard]
    },
    { path: '', redirectTo: 'workouts', pathMatch: 'full' },
    { path: '**', redirectTo: 'workouts' }
];
