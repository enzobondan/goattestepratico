import { Component, OnInit, inject } from '@angular/core';
import {
  FormBuilder,
  FormGroup,
  FormsModule,
  ReactiveFormsModule,
  Validators,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ToastrService } from 'ngx-toastr';
import { ApiService } from '../../core/services/api.service';
import { CreateAccountPayload } from '../../core/interfaces';

export const confirmPasswordValidator: ValidatorFn = (
  control: AbstractControl
): ValidationErrors | null => {
  const password = control.get('password');
  const confirmPassword = control.get('confirmPassword');

  return password && confirmPassword && password.value === confirmPassword.value
    ? null
    : { passwordMismatch: true };
};

@Component({
  selector: 'app-plans',
  templateUrl: './plans.component.html',
  styleUrls: ['./plans.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule,
    RouterModule,
  ],
  standalone: true,
})
export class PlansComponent implements OnInit {
  selectedPlan: 'basic' | 'premium' | 'enterprise' = 'premium';
  planForm: FormGroup;
  isLoading: boolean = false;

  private api = inject(ApiService);
  private toastr = inject(ToastrService);

  constructor(private fb: FormBuilder, private route: ActivatedRoute) {
    this.planForm = this.fb.group(
      {
        razaoSocial: ['', Validators.required],
        cnpj: [
          '',
          [Validators.required, Validators.pattern(/^\d{2}\.\d{3}\.\d{3}\/\d{4}\-\d{2}$/)],
        ],
        nomeFantasia: [''],
        inscricaoEstadual: ['', [Validators.maxLength(12)]],
        nomeCompleto: ['', Validators.required],
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(6)]],
        confirmPassword: ['', Validators.required],
      },
      { validators: confirmPasswordValidator }
    );
  }

  ngOnInit(): void {
    this.route.queryParams.subscribe((params) => {
      const plan = params['plan'];
      if (plan && ['basic', 'premium', 'enterprise'].includes(plan)) {
        this.selectedPlan = plan as 'basic' | 'premium' | 'enterprise';
        setTimeout(() => {
          const formSection = document.getElementById('form-section');
          if (formSection) {
            formSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
          }
        }, 300);
      }
    });
  }

  selectPlan(plan: 'basic' | 'premium' | 'enterprise'): void {
    this.selectedPlan = plan;
    setTimeout(() => {
      const formSection = document.getElementById('form-section');
      if (formSection) {
        formSection.scrollIntoView({ behavior: 'smooth', block: 'start' });
      }
    }, 300);
  }

  onSubmit(): void {
    if (this.planForm.valid) {
      this.isLoading = true;
      const payload: CreateAccountPayload = {
        account: {
          razaoSocial: this.planForm.get('razaoSocial')?.value,
          cnpj: this.planForm.get('cnpj')?.value.replace(/\D/g, ''),
          nomeFantasia: this.planForm.get('nomeFantasia')?.value || undefined,
          inscricaoEstadual: this.planForm.get('inscricaoEstadual')?.value || undefined,
        },
        owner: {
          nomeCompleto: this.planForm.get('nomeCompleto')?.value,
          email: this.planForm.get('email')?.value,
          password: this.planForm.get('password')?.value,
        },
      };

      console.log('Enviando dados para API:', payload);
      this.api
        .post<{ message: string; success: boolean }>('Accounts/create-with-owner', payload)
        .subscribe({
          next: (response) => {
            this.isLoading = false;
            console.log('Resposta da API:', response);
            this.toastr.success(
              'Conta criada com sucesso! Em breve você receberá um email de confirmação.',
              'Sucesso',
              {
                timeOut: 5000,
                positionClass: 'toast-top-right',
              }
            );

            this.planForm.reset();
          },
          error: (error) => {
            this.isLoading = false;
            console.error('Erro ao criar conta:', error);
            let errorMessage = 'Erro ao criar conta. Por favor, tente novamente.';

            if (error.message.includes('CNPJ já cadastrado')) {
              errorMessage = 'CNPJ já cadastrado em nossa base de dados.';
            } else if (error.message.includes('Email já cadastrado')) {
              errorMessage = 'Email já cadastrado em nossa base de dados.';
            } else if (error.message) {
              errorMessage = error.message;
            }

            this.toastr.error(errorMessage, 'Erro', {
              timeOut: 5000,
              positionClass: 'toast-top-right',
            });
          },
        });
    } else {
      Object.keys(this.planForm.controls).forEach((key) => {
        this.planForm.get(key)?.markAsTouched();
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
  isFieldInvalid(fieldName: string): boolean {
    const field = this.planForm.get(fieldName);
    return field ? field.invalid && field.touched : false;
  }

  formatCnpj(event: any): void {
    let value = event.target.value.replace(/\D/g, '');

    if (value.length > 14) {
      value = value.substring(0, 14);
    }

    if (value.length > 12) {
      value = value.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/, '$1.$2.$3/$4-$5');
    } else if (value.length > 8) {
      value = value.replace(/^(\d{2})(\d{3})(\d{3})(\d{4})/, '$1.$2.$3/$4');
    } else if (value.length > 5) {
      value = value.replace(/^(\d{2})(\d{3})(\d{3})/, '$1.$2.$3');
    } else if (value.length > 2) {
      value = value.replace(/^(\d{2})(\d{3})/, '$1.$2');
    }

    this.planForm.patchValue({ cnpj: value }, { emitEvent: false });
  }

  formatPhone(event: any): void {
    let value = event.target.value.replace(/\D/g, '');

    if (value.length > 11) {
      value = value.substring(0, 11);
    }

    if (value.length > 10) {
      value = value.replace(/^(\d{2})(\d{5})(\d{4})/, '($1) $2-$3');
    } else if (value.length > 6) {
      value = value.replace(/^(\d{2})(\d{4})(\d{4})/, '($1) $2-$3');
    } else if (value.length > 2) {
      value = value.replace(/^(\d{2})(\d{0,4})/, '($1) $2');
    } else if (value.length > 0) {
      value = value.replace(/^(\d*)/, '($1');
    }

    this.planForm.patchValue({ phone: value }, { emitEvent: false });
  }
}
