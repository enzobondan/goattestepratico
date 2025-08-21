import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Tenant } from '../../core/interfaces';

@Component({
  selector: 'app-tenant-control',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './tenant-control.component.html',
  styleUrls: ['./tenant-control.component.css'],
})
export class TenantControlComponent {
  @Input() showModal: boolean = false;
  @Input() isEditMode: boolean = false;
  @Input() tenant: Tenant = {
    accountId: '',
    id: '',
    razaoSocial: '',
    cnpj: '',
    nomeFantasia: '',
    limiteAprovacaoAutomatica: 0,
    ativo: true,
  };

  @Output() close = new EventEmitter<void>();
  @Output() save = new EventEmitter<Tenant>();
  @Output() delete = new EventEmitter<string>();

  limiteFormatado: string = '';

  get title(): string {
    return this.isEditMode ? 'Editar Tenant' : 'Criar Novo Tenant';
  }

  ngOnInit() {
    if (this.tenant.limiteAprovacaoAutomatica) {
      this.limiteFormatado = this.formatCurrency(this.tenant.limiteAprovacaoAutomatica);
    }
  }

  onSubmit() {
    if (this.limiteFormatado) {
      this.tenant.limiteAprovacaoAutomatica = this.parseCurrency(this.limiteFormatado);
    }
    this.save.emit(this.tenant);
  }

  onClose() {
    this.close.emit();
  }

  onDelete() {
    if (confirm('Tem certeza que deseja excluir este tenant? Esta ação não pode ser desfeita.')) {
      this.delete.emit(this.tenant.id);
    }
  }

  formatCnpj(event: any): void {
    let input = event.target;
    let value = input.value.replace(/\D/g, '');

    if (value.length > 14) {
      value = value.substring(0, 14);
    }

    if (value.length <= 2) {
      value = value;
    } else if (value.length <= 5) {
      value = `${value.substring(0, 2)}.${value.substring(2)}`;
    } else if (value.length <= 8) {
      value = `${value.substring(0, 2)}.${value.substring(2, 5)}.${value.substring(5)}`;
    } else if (value.length <= 12) {
      value = `${value.substring(0, 2)}.${value.substring(2, 5)}.${value.substring(
        5,
        8
      )}/${value.substring(8)}`;
    } else {
      value = `${value.substring(0, 2)}.${value.substring(2, 5)}.${value.substring(
        5,
        8
      )}/${value.substring(8, 12)}-${value.substring(12)}`;
    }

    input.value = value;
    this.tenant.cnpj = value;
  }

  onCnpjKeypress(event: KeyboardEvent): void {
    const allowedKeys = ['Backspace', 'Tab', 'ArrowLeft', 'ArrowRight', 'Delete'];
    if (!/^\d$/.test(event.key) && !allowedKeys.includes(event.key)) {
      event.preventDefault();
    }
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2,
    })
      .format(value)
      .replace('R$', '')
      .trim();
  }

  parseCurrency(formattedValue: string): number {
    const cleanValue = formattedValue.replace(/\./g, '').replace(',', '.').replace(/\s/g, '');
    return parseFloat(cleanValue) || 0;
  }

  onLimiteInput(event: any): void {
    let value = event.target.value.replace(/\D/g, '');

    const numericValue = parseFloat(value) / 100;

    if (!isNaN(numericValue)) {
      this.limiteFormatado = this.formatCurrency(numericValue);
      this.tenant.limiteAprovacaoAutomatica = numericValue;
    } else {
      this.limiteFormatado = '';
      this.tenant.limiteAprovacaoAutomatica = 0;
    }
  }

  onLimiteBlur(): void {
    if (this.tenant.limiteAprovacaoAutomatica === 0) {
      this.limiteFormatado = '';
    }
  }
}
