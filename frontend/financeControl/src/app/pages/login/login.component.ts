import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
} from '@angular/forms';
import { Router, RouterModule, ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../core/services/auth.service';
import { LoginRequest } from '../../core/interfaces';
import { ApiService } from '../../core/services/api.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterModule],
  standalone: true,
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  forgotPasswordForm: FormGroup;
  resetPasswordForm: FormGroup;
  isLoading: boolean = false;
  isForgotPasswordMode: boolean = false;
  isResetPasswordMode: boolean = false;
  returnUrl: string = '/dashboard';
  resetToken: string = '';

  private authService = inject(AuthService);
  private apiService = inject(ApiService);
  private toastr = inject(ToastrService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);

  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      rememberMe: [false],
    });

    this.forgotPasswordForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
    });

    this.resetPasswordForm = this.fb.group(
      {
        newPassword: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', [Validators.required]],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  ngOnInit(): void {
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/dashboard';
    if (this.authService.isAuthenticated()) {
      this.router.navigate([this.returnUrl]);
    }
    this.fillRememberedEmail();
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('newPassword');
    const confirmPassword = form.get('confirmPassword');

    if (password && confirmPassword && password.value !== confirmPassword.value) {
      confirmPassword.setErrors({ passwordMismatch: true });
    } else {
      confirmPassword?.setErrors(null);
    }
    return null;
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.isLoading = true;

      const credentials: LoginRequest = {
        email: this.loginForm.get('email')?.value,
        password: this.loginForm.get('password')?.value,
      };

      this.authService.login(credentials).subscribe({
        next: (response) => {
          this.isLoading = false;
          this.toastr.success('Login realizado com sucesso!', 'Bem-vindo', {
            timeOut: 3000,
            positionClass: 'toast-top-right',
          });
          if (this.loginForm.get('rememberMe')?.value) {
            localStorage.setItem('rememberedEmail', credentials.email);
          } else {
            localStorage.removeItem('rememberedEmail');
          }
          this.router.navigate([this.returnUrl]);
        },
        error: (error) => {
          this.isLoading = false;
          let errorMessage = 'Erro ao fazer login. Por favor, tente novamente.';

          if (error.status === 401) {
            errorMessage = 'Email ou senha incorretos.';
          } else if (error.status === 0) {
            errorMessage = 'Não foi possível conectar ao servidor. Verifique sua conexão.';
          } else if (error.error?.message) {
            errorMessage = error.error.message;
          }

          this.toastr.error(errorMessage, 'Erro no login', {
            timeOut: 5000,
            positionClass: 'toast-top-right',
          });
        },
      });
    } else {
      Object.keys(this.loginForm.controls).forEach((key) => {
        this.loginForm.get(key)?.markAsTouched();
      });

      this.toastr.warning(
        'Por favor, preencha todos os campos obrigatórios corretamente.',
        'Atenção',
        {
          timeOut: 5000,
          positionClass: 'toast-top-right',
        }
      );
    }
  }

  onForgotPassword(): void {
    if (this.forgotPasswordForm.valid) {
      this.isLoading = true;
      const email = this.forgotPasswordForm.get('email')?.value;

      this.apiService.post('Auth/forgot-password', { email }).subscribe({
        next: (response: any) => {
          this.isLoading = false;
          this.resetToken = response.message;
          this.isForgotPasswordMode = false;
          this.isResetPasswordMode = true;

          this.toastr.success('Token de recuperação enviado!', 'Sucesso', {
            timeOut: 3000,
            positionClass: 'toast-top-right',
          });
        },
        error: (error) => {
          this.isLoading = false;
          let errorMessage = 'Erro ao solicitar recuperação de senha.';

          if (error.status === 404) {
            errorMessage = 'Email não encontrado.';
          } else if (error.status === 0) {
            errorMessage = 'Não foi possível conectar ao servidor.';
          } else if (error.error?.message) {
            errorMessage = error.error.message;
          }

          this.toastr.error(errorMessage, 'Erro', {
            timeOut: 5000,
            positionClass: 'toast-top-right',
          });
        },
      });
    }
  }

  onResetPassword(): void {
    if (this.resetPasswordForm.valid) {
      this.isLoading = true;
      const newPassword = this.resetPasswordForm.get('newPassword')?.value;

      this.apiService
        .post('Auth/reset-password', {
          token: this.resetToken,
          newPassword: newPassword,
        })
        .subscribe({
          next: (response: any) => {
            console.log(response);
            this.isLoading = false;
            this.isResetPasswordMode = false;

            this.toastr.success('Senha redefinida com sucesso!', 'Sucesso', {
              timeOut: 3000,
              positionClass: 'toast-top-right',
            });
          },
          error: (error) => {
            this.isLoading = false;
            let errorMessage = 'Erro ao redefinir senha.';

            if (error.status === 400) {
              errorMessage = 'Token inválido ou expirado.';
            } else if (error.status === 0) {
              errorMessage = 'Não foi possível conectar ao servidor.';
            } else if (error.error?.message) {
              errorMessage = error.error.message;
            }

            this.toastr.error(errorMessage, 'Erro', {
              timeOut: 5000,
              positionClass: 'toast-top-right',
            });
          },
        });
    }
  }

  showForgotPassword(): void {
    this.isForgotPasswordMode = true;
    this.isResetPasswordMode = false;
    const loginEmail = this.loginForm.get('email')?.value;
    if (loginEmail) {
      this.forgotPasswordForm.patchValue({ email: loginEmail });
    }
  }

  backToLogin(): void {
    this.isForgotPasswordMode = false;
    this.isResetPasswordMode = false;
    this.resetToken = '';
    this.forgotPasswordForm.reset();
    this.resetPasswordForm.reset();
  }

  isFieldInvalid(form: FormGroup, fieldName: string): boolean {
    const field = form.get(fieldName);
    return field ? field.invalid && (field.touched || field.dirty) : false;
  }

  isLoginFieldInvalid(fieldName: string): boolean {
    return this.isFieldInvalid(this.loginForm, fieldName);
  }

  fillRememberedEmail(): void {
    const rememberedEmail = localStorage.getItem('rememberedEmail');
    if (rememberedEmail) {
      this.loginForm.patchValue({
        email: rememberedEmail,
        rememberMe: true,
      });
    }
  }
}
