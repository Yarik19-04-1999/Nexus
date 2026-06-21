export type RewatchStatus = 0 | 1 | 2

export const REWATCH_STATUS_LABELS: Record<RewatchStatus, string> = {
  0: 'Must rewatch',
  1: 'Optional',
  2: 'Not worth it',
}

export const REWATCH_STATUS_COLORS: Record<RewatchStatus, string> = {
  0: 'bg-green-100 text-green-800',
  1: 'bg-yellow-100 text-yellow-800',
  2: 'bg-red-100 text-red-800',
}

export interface Movie {
  id: number
  title: string
  releaseYear: number
  durationMinutes: number
  reviewText: string | null
  score: number | null
  viewCount: number
  rewatchStatus: RewatchStatus
  universeId: number | null
  universeName: string | null
  listNo: number
  createdAt: string
  updatedAt: string
}

export interface SearchMovieResult {
  id: number
  title: string
  releaseYear: number
}
