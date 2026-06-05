export function validateCpf(cpf: string | null | undefined): boolean {
  if (!cpf) return false;

  const clean = cpf.replace(/\D/g, '');

  if (clean.length !== 11) return false;

  if (/^(\d)\1+$/.test(clean)) return false;

  const multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
  const multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

  let tempCpf = clean.substring(0, 9);
  let soma = 0;

  for (let i = 0; i < 9; i++) {
    soma += parseInt(tempCpf.charAt(i), 10) * multiplicador1[i];
  }

  let resto = soma % 11;
  resto = resto < 2 ? 0 : 11 - resto;

  let digito = resto.toString();
  tempCpf += digito;
  soma = 0;

  for (let i = 0; i < 10; i++) {
    soma += parseInt(tempCpf.charAt(i), 10) * multiplicador2[i];
  }

  resto = soma % 11;
  resto = resto < 2 ? 0 : 11 - resto;
  digito += resto.toString();

  return clean.endsWith(digito);
}

export function formatCpf(cpf: string | null | undefined): string {
  if (!cpf) return '';
  const clean = cpf.replace(/\D/g, '');
  if (clean.length < 11) {
    if (clean.length <= 3) return clean;
    if (clean.length <= 6) return `${clean.substring(0, 3)}.${clean.substring(3)}`;
    if (clean.length <= 9) return `${clean.substring(0, 3)}.${clean.substring(3, 6)}.${clean.substring(6)}`;
    return `${clean.substring(0, 3)}.${clean.substring(3, 6)}.${clean.substring(6, 9)}-${clean.substring(9)}`;
  }
  return `${clean.substring(0, 3)}.${clean.substring(3, 6)}.${clean.substring(6, 9)}-${clean.substring(9, 11)}`;
}

export function cleanCpf(cpf: string | null | undefined): string {
  if (!cpf) return '';
  return cpf.replace(/\D/g, '');
}
