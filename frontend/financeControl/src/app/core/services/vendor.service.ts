import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ApiService } from './api.service';
import { Vendor } from '../interfaces';

@Injectable({
  providedIn: 'root',
})
export class VendorService {
  constructor(private api: ApiService) {}

  getVendorsByTenant(tenantId: string): Observable<Vendor[]> {
    return this.api.get<Vendor[]>(`Vendors/tenant/${tenantId}`);
  }

  getVendorById(id: string): Observable<Vendor> {
    return this.api.get<Vendor>(`Vendors/${id}`);
  }

  createVendor(vendor: Partial<Vendor>): Observable<Vendor> {
    return this.api.post<Vendor>('Vendors', vendor);
  }

  updateVendor(id: string, vendor: Partial<Vendor>): Observable<Vendor> {
    return this.api.put<Vendor>(`Vendors/${id}`, vendor);
  }

  deleteVendor(id: string): Observable<void> {
    return this.api.delete<void>(`Vendors/${id}`);
  }
}
