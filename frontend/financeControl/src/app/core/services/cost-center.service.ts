import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { CostCenter } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class CostCenterService {
  constructor(private api: ApiService) {}

  getCostCentersByTenant(tenantId: string): Observable<CostCenter[]> {
    return this.api.get<CostCenter[]>(`CostCenters/tenant/${tenantId}`);
  }

  getCostCenterById(id: string): Observable<CostCenter> {
    return this.api.get<CostCenter>(`CostCenters/${id}`);
  }

  createCostCenter(costCenter: Partial<CostCenter>): Observable<CostCenter> {
    return this.api.post<CostCenter>('CostCenters', costCenter);
  }

  updateCostCenter(id: string, costCenter: Partial<CostCenter>): Observable<CostCenter> {
    return this.api.put<CostCenter>(`CostCenters/${id}`, costCenter);
  }

  deleteCostCenter(id: string): Observable<void> {
    return this.api.delete<void>(`CostCenters/${id}`);
  }
}
