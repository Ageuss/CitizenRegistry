import { Component, OnInit, signal, computed, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CitizenService, Citizen } from './services/citizen.service';
import { validateCpf, formatCpf, cleanCpf } from './utils/cpf-validator';
import { catchError, of } from 'rxjs';

@Component({
  selector: 'app-root',
  imports: [CommonModule, FormsModule],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App implements OnInit {
  private citizenService = inject(CitizenService);

  protected readonly title = signal('Registro de Cidadãos');
  protected readonly citizens = signal<Citizen[]>([]);
  protected readonly loading = signal<boolean>(false);
  protected readonly submitting = signal<boolean>(false);

  protected readonly searchQuery = signal<string>('');
  protected readonly searchType = signal<'all' | 'name' | 'cpf'>('all');

  protected readonly formName = signal<string>('');
  protected readonly formCpf = signal<string>('');

  protected readonly toasts = signal<Array<{ id: number; message: string; type: 'success' | 'error' | 'info' }>>([]);
  private toastIdCounter = 0;

  protected readonly isCpfValid = computed(() => {
    const val = this.formCpf();
    if (!val) return false;
    return validateCpf(val);
  });

  protected readonly isNameValid = computed(() => {
    const val = this.formName();
    return val.trim().length > 0 && val.trim().length <= 150;
  });

  protected readonly isFormValid = computed(() => {
    return this.isNameValid() && this.isCpfValid();
  });

  ngOnInit() {
    this.loadAllCitizens();
  }

  loadAllCitizens() {
    this.loading.set(true);
    this.citizenService.getAll().pipe(
      catchError(err => {
        console.error(err);
        this.showToast('Erro ao carregar cidadãos. Verifique se a API está executando.', 'error');
        return of([]);
      })
    ).subscribe(data => {
      this.citizens.set(data);
      this.loading.set(false);
    });
  }

  onSearch() {
    const query = this.searchQuery().trim();
    const type = this.searchType();

    if (!query) {
      this.loadAllCitizens();
      return;
    }

    this.loading.set(true);

    if (type === 'cpf') {
      const cleaned = cleanCpf(query);
      this.citizenService.getByCpf(cleaned).pipe(
        catchError(err => {
          if (err.status === 404) {
            return of(null);
          }
          this.showToast('CPF não encontrado.', 'info');
          return of(null);
        })
      ).subscribe(data => {
        this.citizens.set(data ? [data] : []);
        this.loading.set(false);
      });
    } else if (type === 'name') {
      this.citizenService.getByName(query).pipe(
        catchError(err => {
          if (err.status === 404) {
            return of([]);
          }
          this.showToast('Nenhum cidadão encontrado com esse nome.', 'info');
          return of([]);
        })
      ).subscribe(data => {
        this.citizens.set(data || []);
        this.loading.set(false);
      });
    } else {
      this.citizenService.getAll().pipe(
        catchError(err => {
          console.error(err);
          return of([]);
        })
      ).subscribe(data => {
        const lowerQuery = query.toLowerCase();
        const filtered = data.filter(c =>
          (c.name?.toLowerCase().includes(lowerQuery)) ||
          (c.cpf?.includes(query))
        );
        this.citizens.set(filtered);
        this.loading.set(false);
      });
    }
  }

  onClearSearch() {
    this.searchQuery.set('');
    this.loadAllCitizens();
  }

  onCpfInput(event: Event) {
    const input = event.target as HTMLInputElement;
    const formatted = formatCpf(input.value);
    this.formCpf.set(formatted);
    input.value = formatted;
  }

  onSubmit() {
    if (!this.isFormValid()) return;

    this.submitting.set(true);
    const newCitizen: Citizen = {
      name: this.formName().trim(),
      cpf: cleanCpf(this.formCpf())
    };

    this.citizenService.create(newCitizen).pipe(
      catchError(err => {
        this.submitting.set(false);
        let errMsg = 'Erro ao cadastrar cidadão.';
        if (err.error && err.error.errors) {
          errMsg = err.error.errors.join(' ');
        } else if (typeof err.error === 'string') {
          errMsg = err.error;
        }
        this.showToast(errMsg, 'error');
        return of(null);
      })
    ).subscribe(result => {
      if (result) {
        this.showToast('Cidadão cadastrado com sucesso!', 'success');
        this.formName.set('');
        this.formCpf.set('');
        this.loadAllCitizens();
      }
      this.submitting.set(false);
    });
  }

  protected formatCpfValue(val: string): string {
    return formatCpf(val);
  }

  protected getInitials(name: string): string {
    if (!name) return 'C';
    const parts = name.trim().split(/\s+/);
    if (parts.length > 1) {
      return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
    }
    return parts[0].substring(0, 2).toUpperCase();
  }

  showToast(message: string, type: 'success' | 'error' | 'info' = 'info') {
    const id = this.toastIdCounter++;
    const newToast = { id, message, type };
    this.toasts.update(current => [...current, newToast]);

    setTimeout(() => {
      this.removeToast(id);
    }, 4500);
  }

  removeToast(id: number) {
    this.toasts.update(current => current.filter(t => t.id !== id));
  }
}
