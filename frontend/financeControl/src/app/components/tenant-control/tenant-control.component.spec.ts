import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TenantControlComponent } from './tenant-control.component';

describe('TenantControlComponent', () => {
  let component: TenantControlComponent;
  let fixture: ComponentFixture<TenantControlComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TenantControlComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TenantControlComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
