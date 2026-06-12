import { API_BASE_URL } from '@/lib/constants'
import type { DomainError } from '@/types/invite'

export class ApiError extends Error {
  constructor(
    public readonly status: number,
    public readonly domain?: DomainError,
  ) {
    super(domain?.errorMessage ?? `HTTP ${status}`)
  }
}

async function request<T>(path: string, init?: RequestInit): Promise<T> {
  const res = await fetch(`${API_BASE_URL}${path}`, {
    headers: { 'Content-Type': 'application/json' },
    ...init,
  })

  if (!res.ok) {
    const domain = res.status === 418 ? ((await res.json()) as DomainError) : undefined
    throw new ApiError(res.status, domain)
  }

  if (res.status === 204) {
    return undefined as T
  }

  return res.json() as Promise<T>
}

export const apiClient = {
  get: <T>(path: string) => request<T>(path),
  post: <T>(path: string, body?: unknown) =>
    request<T>(path, { method: 'POST', body: body !== undefined ? JSON.stringify(body) : undefined }),
  put: <T>(path: string, body: unknown) =>
    request<T>(path, { method: 'PUT', body: JSON.stringify(body) }),
  delete: (path: string) => request<void>(path, { method: 'DELETE' }),
}
