import { Component, ElementRef } from '@angular/core';
import { NgStyle } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'app-home',
  imports: [NgStyle],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css',
})
export class HomeComponent {
  heroImage: string = 'img1.png';

  constructor(private elementRef: ElementRef, private router: Router) {}

  scrollToSection(sectionId: string): void {
    const element = this.elementRef.nativeElement.querySelector(`#${sectionId}`);
    if (element) {
      element.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }

  planosAccess(opcao: string) {
    this.router.navigate(['/planos'], {
      queryParams: { plan: opcao },
    });
  }
  loginAccess() {
    this.router.navigate(['/dashboard']);
  }
}
