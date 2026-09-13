import { Component, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [ReactiveFormsModule, RouterLink],
    templateUrl: './login.component.html',
    styleUrl: './login.component.scss'
})
export class LoginComponent {
    form: FormGroup;
    errorMessage = signal('');
    isSubmitting = signal(false);

    constructor(
        private fb: FormBuilder,
        private authService: AuthService,
        private router: Router
    ) {
        this.form = this.fb.group({
            email: ['', [Validators.required, Validators.email]],
            password: ['', Validators.required]
        });
    }

    onSubmit(): void {
        if(this.form.invalid || this.isSubmitting()){
            return;
        }

        this.errorMessage.set('');
        this.isSubmitting.set(true);

        this.authService.login(this.form.value).subscribe({
            next: () => this.router.navigateByUrl('/workouts'),
            error: () => {
                this.errorMessage.set("Wrong email or password.");
                this.isSubmitting.set(false);
            }
        });
    }
    
}