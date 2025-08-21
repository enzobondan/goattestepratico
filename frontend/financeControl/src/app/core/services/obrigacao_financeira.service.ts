import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ApiService } from './api.service';
import {
  ObrigacaoFinanceira,
  PaginatedResponse,
  ObrigacaoFilter,
  RegistroAprovacao,
  Anexo,
} from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class ObrigacaoFinanceiraService {
  constructor(private api: ApiService) {}

  getObrigacoes(filter: ObrigacaoFilter = {}): Observable<PaginatedResponse<ObrigacaoFinanceira>> {
    return this.api.get<PaginatedResponse<ObrigacaoFinanceira>>('obrigacoes-financeiras', filter);
  }

  getObrigacaoById(id: string): Observable<ObrigacaoFinanceira> {
    return this.api.get<ObrigacaoFinanceira>(`obrigacoes-financeiras/${id}`);
  }

  createObrigacao(obrigacao: Partial<ObrigacaoFinanceira>): Observable<ObrigacaoFinanceira> {
    return this.api.post<ObrigacaoFinanceira>('obrigacoes-financeiras', obrigacao);
  }

  updateObrigacao(
    id: string,
    obrigacao: Partial<ObrigacaoFinanceira>
  ): Observable<ObrigacaoFinanceira> {
    return this.api.put<ObrigacaoFinanceira>(`obrigacoes-financeiras/${id}`, obrigacao);
  }

  deleteObrigacao(id: string): Observable<void> {
    return this.api.delete<void>(`obrigacoes-financeiras/${id}`);
  }

  changeStatus(id: string, status: string): Observable<ObrigacaoFinanceira> {
    return this.api.patch<ObrigacaoFinanceira>(`obrigacoes-financeiras/${id}/status`, { status });
  }

  submitForApproval(id: string): Observable<ObrigacaoFinanceira> {
    return this.api.post<ObrigacaoFinanceira>(`obrigacoes-financeiras/${id}/submit-approval`, {});
  }

  approveObrigacao(id: string, comentario?: string): Observable<RegistroAprovacao> {
    return this.api.post<RegistroAprovacao>(`obrigacoes-financeiras/${id}/approve`, { comentario });
  }

  rejectObrigacao(id: string, comentario: string): Observable<RegistroAprovacao> {
    return this.api.post<RegistroAprovacao>(`obrigacoes-financeiras/${id}/reject`, { comentario });
  }

  getAprovacoes(id: string): Observable<RegistroAprovacao[]> {
    return this.api.get<RegistroAprovacao[]>(`obrigacoes-financeiras/${id}/approvals`);
  }

  uploadAnexo(id: string, file: File): Observable<Anexo> {
    const formData = new FormData();
    formData.append('file', file);
    return this.api.upload<Anexo>(`obrigacoes-financeiras/${id}/attachments`, formData);
  }

  deleteAnexo(obrigacaoId: string, anexoId: string): Observable<void> {
    return this.api.delete<void>(`obrigacoes-financeiras/${obrigacaoId}/attachments/${anexoId}`);
  }

  getAnexos(id: string): Observable<Anexo[]> {
    return this.api.get<Anexo[]>(`obrigacoes-financeiras/${id}/attachments`);
  }

  getDashboardData(): Observable<any> {
    return this.api.get<any>('obrigacoes-financeiras/dashboard');
  }
}
