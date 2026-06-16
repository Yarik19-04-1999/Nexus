export enum InviteAnswer {
  Pending = 0,
  Yes = 1,
  No = 2,
}

export enum InviteLanguage {
  Russian = 0,
  Ukrainian = 1,
  English = 2,
}

export enum InviteMascot {
  MochiPeachCat = 0,
  UtyaDuck = 1,
}

export interface Invite {
  id: number
  code: string
  message: string
  description?: string
  expiresAt?: string
  answer: InviteAnswer
  language: InviteLanguage
  mascot: InviteMascot
  createdAt: string
  updatedAt: string
}

export interface PagedResult<T> {
  items: T[]
  totalCount: number
  page: number
  pageSize: number
  totalPages: number
}

export interface DomainError {
  errorCode: string
  errorMessage?: string
  canRetry: boolean
}
