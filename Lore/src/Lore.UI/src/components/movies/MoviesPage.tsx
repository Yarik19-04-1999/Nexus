'use client'

import { useState, useCallback } from 'react'
import { useSearchParams, useRouter, usePathname } from 'next/navigation'
import { Plus } from 'lucide-react'
import { useMoviesList, useCreateMovie, useUpdateMovie, useDeleteMovie } from '@/hooks/useMovies'
import { MovieModal } from './MovieModal'
import { MovieFilters } from './MovieFilters'
import { ViewCountControls } from './ViewCountControls'
import { LinkUniverseModal } from './LinkUniverseModal'
import { Pagination } from '@/components/universes/Pagination'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { Spinner } from '@/components/ui/Spinner'
import { REWATCH_STATUS_LABELS, REWATCH_STATUS_COLORS } from '@/types/movie'
import type { Movie, RewatchStatus } from '@/types/movie'
import type { CreateMoviePayload } from '@/lib/api/movies'
import { DEFAULT_PAGE_SIZE } from '@/lib/constants'
import Link from 'next/link'

interface Filters {
  rewatchStatuses: RewatchStatus[]
  scoreMin: string
  scoreMax: string
  yearMin: string
  yearMax: string
}

function buildSieveFilters(f: Filters): string {
  const parts: string[] = []
  if (f.rewatchStatuses.length > 0) {
    parts.push(`RewatchStatus==${f.rewatchStatuses.join('|')}`)
  }
  if (f.scoreMin) parts.push(`Score>=${f.scoreMin}`)
  if (f.scoreMax) parts.push(`Score<=${f.scoreMax}`)
  if (f.yearMin) parts.push(`ReleaseYear>=${f.yearMin}`)
  if (f.yearMax) parts.push(`ReleaseYear<=${f.yearMax}`)
  return parts.join(',')
}

export function MoviesPage() {
  const router = useRouter()
  const pathname = usePathname()
  const searchParams = useSearchParams()

  const page = Number(searchParams.get('page') ?? '1')
  const pageSize = Number(searchParams.get('pageSize') ?? String(DEFAULT_PAGE_SIZE))

  const [filters, setFilters] = useState<Filters>({
    rewatchStatuses: [], scoreMin: '', scoreMax: '', yearMin: '', yearMax: '',
  })
  const [createOpen, setCreateOpen] = useState(false)
  const [editTarget, setEditTarget] = useState<Movie | null>(null)
  const [deleteTarget, setDeleteTarget] = useState<Movie | null>(null)
  const [linkTarget, setLinkTarget] = useState<Movie | null>(null)

  const updateParams = useCallback((updates: Record<string, string | null>) => {
    const params = new URLSearchParams(searchParams.toString())
    Object.entries(updates).forEach(([k, v]) => {
      if (v === null) params.delete(k)
      else params.set(k, v)
    })
    if (params.get('page') === '1') params.delete('page')
    router.replace(`${pathname}?${params.toString()}`, { scroll: false })
  }, [searchParams, pathname, router])

  const sieveFilters = buildSieveFilters(filters)
  const { data, isPending } = useMoviesList({ page, pageSize, filters: sieveFilters || undefined })

  const createMovie = useCreateMovie()
  const updateMovie = useUpdateMovie()
  const deleteMovie = useDeleteMovie()

  const handleCreate = async (payload: CreateMoviePayload) => {
    await createMovie.mutateAsync(payload)
    setCreateOpen(false)
  }

  const handleUpdate = async (payload: CreateMoviePayload) => {
    if (!editTarget) return
    await updateMovie.mutateAsync({ id: editTarget.id, payload })
    setEditTarget(null)
  }

  const handleDelete = async () => {
    if (!deleteTarget) return
    await deleteMovie.mutateAsync(deleteTarget.id)
    setDeleteTarget(null)
  }

  return (
    <main className="px-6 py-8">
      <div className="flex items-center justify-between mb-6">
        <h1 className="text-xl font-bold text-gray-800">Movies</h1>
        <button
          onClick={() => setCreateOpen(true)}
          className="flex items-center gap-2 px-4 py-2 rounded-xl text-sm font-medium text-white bg-indigo-500 hover:bg-indigo-600 transition-colors"
        >
          <Plus className="w-4 h-4" />
          New movie
        </button>
      </div>

      <div className="flex gap-8 items-start">
        <div className="flex-1 min-w-0">
          {isPending ? (
            <div className="flex justify-center py-12"><Spinner /></div>
          ) : !data?.items.length ? (
            <p className="text-center text-gray-400 py-12">No movies yet</p>
          ) : (
            <>
              <div className="overflow-x-auto rounded-2xl border border-gray-100">
                <table className="w-full text-sm">
                  <thead>
                    <tr className="bg-gray-50 text-left text-gray-500 text-xs uppercase tracking-wide">
                      <th className="px-4 py-3 font-medium">Title</th>
                      <th className="px-4 py-3 font-medium hidden sm:table-cell">Year</th>
                      <th className="px-4 py-3 font-medium hidden md:table-cell">Duration</th>
                      <th className="px-4 py-3 font-medium">Score</th>
                      <th className="px-4 py-3 font-medium hidden lg:table-cell">Status</th>
                      <th className="px-4 py-3 font-medium">Views</th>
                      <th className="px-4 py-3 font-medium hidden md:table-cell">Universe</th>
                      <th className="px-4 py-3" />
                    </tr>
                  </thead>
                  <tbody className="divide-y divide-gray-50">
                    {data.items.map(movie => (
                      <tr key={movie.id} className="hover:bg-gray-50/50 transition-colors">
                        <td className="px-4 py-3 font-medium text-gray-800 max-w-[200px]">
                          <span className="truncate block">{movie.title}</span>
                        </td>
                        <td className="px-4 py-3 text-gray-500 hidden sm:table-cell">{movie.releaseYear}</td>
                        <td className="px-4 py-3 text-gray-500 hidden md:table-cell">{movie.durationMinutes} min</td>
                        <td className="px-4 py-3 text-gray-700 tabular-nums">
                          {movie.score != null ? movie.score.toFixed(1) : <span className="text-gray-300">—</span>}
                        </td>
                        <td className="px-4 py-3 hidden lg:table-cell">
                          <span className={`inline-flex px-2 py-0.5 rounded-full text-xs font-medium ${REWATCH_STATUS_COLORS[movie.rewatchStatus]}`}>
                            {REWATCH_STATUS_LABELS[movie.rewatchStatus]}
                          </span>
                        </td>
                        <td className="px-4 py-3">
                          <ViewCountControls movieId={movie.id} viewCount={movie.viewCount} />
                        </td>
                        <td className="px-4 py-3 hidden md:table-cell">
                          {movie.universeName ? (
                            <div className="flex items-center gap-1.5 min-w-0">
                              <Link
                                href={`/universes/${movie.universeId}`}
                                className="text-indigo-600 hover:text-indigo-800 text-xs truncate max-w-[120px] transition-colors"
                                title={movie.universeName}
                              >
                                {movie.universeName}
                              </Link>
                              <button
                                onClick={() => setLinkTarget(movie)}
                                title="Change or unlink universe"
                                className="shrink-0 text-gray-300 hover:text-gray-500 transition-colors"
                              >
                                <svg className="w-3 h-3" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2}>
                                  <path strokeLinecap="round" strokeLinejoin="round" d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101" />
                                  <path strokeLinecap="round" strokeLinejoin="round" d="M14.828 14.828a4 4 0 015.656 0l-4-4a4 4 0 01-5.656-5.656l1.102-1.101" />
                                </svg>
                              </button>
                            </div>
                          ) : (
                            <button
                              onClick={() => setLinkTarget(movie)}
                              title="Link to universe"
                              className="text-xs text-gray-400 hover:text-indigo-500 transition-colors flex items-center gap-1"
                            >
                              <svg className="w-3 h-3" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2}>
                                <path strokeLinecap="round" strokeLinejoin="round" d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101m3-2.001a4 4 0 015.656 0l-4 4a4 4 0 01-5.656-5.656l1.1-1.1" />
                              </svg>
                              Link
                            </button>
                          )}
                        </td>
                        <td className="px-4 py-3">
                          <div className="flex items-center gap-1 justify-end">
                            <ActionBtn onClick={() => setEditTarget(movie)} title="Edit">
                              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                            </ActionBtn>
                            <ActionBtn onClick={() => setDeleteTarget(movie)} title="Delete" danger>
                              <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M19 7l-.867 12.142A2 2 0 0116.138 21H7.862a2 2 0 01-1.995-1.858L5 7m5 4v6m4-6v6m1-10V4a1 1 0 00-1-1h-4a1 1 0 00-1 1v3M4 7h16" /></svg>
                            </ActionBtn>
                          </div>
                        </td>
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
              <Pagination
                page={page}
                pageSize={pageSize}
                totalPages={data.totalPages}
                totalCount={data.totalCount}
                onPageChange={p => updateParams({ page: String(p) })}
                onPageSizeChange={size => updateParams({ pageSize: String(size), page: '1' })}
              />
            </>
          )}
        </div>

        <MovieFilters filters={filters} onChange={setFilters} />
      </div>

      <MovieModal
        open={createOpen}
        isPending={createMovie.isPending}
        onClose={() => setCreateOpen(false)}
        onSubmit={handleCreate}
      />
      <MovieModal
        open={!!editTarget}
        movie={editTarget}
        isPending={updateMovie.isPending}
        onClose={() => setEditTarget(null)}
        onSubmit={handleUpdate}
      />
      <ConfirmDialog
        open={!!deleteTarget}
        title="Delete movie?"
        description={`"${deleteTarget?.title}" will be permanently deleted.`}
        confirmLabel="Delete"
        cancelLabel="Cancel"
        isDanger
        isPending={deleteMovie.isPending}
        onConfirm={handleDelete}
        onCancel={() => setDeleteTarget(null)}
      />
      <LinkUniverseModal
        movie={linkTarget}
        onClose={() => setLinkTarget(null)}
      />
    </main>
  )
}

function ActionBtn({ onClick, title, danger = false, children }: { onClick: () => void; title: string; danger?: boolean; children: React.ReactNode }) {
  return (
    <button onClick={onClick} title={title} className={`p-2 rounded-lg transition-colors ${danger ? 'text-gray-400 hover:text-rose-500 hover:bg-rose-50' : 'text-gray-400 hover:text-gray-700 hover:bg-gray-100'}`}>
      {children}
    </button>
  )
}
