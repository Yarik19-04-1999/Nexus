'use client'

import { useState } from 'react'
import Link from 'next/link'
import { ArrowLeft, EyeOff, Plus, Link2 } from 'lucide-react'
import { useUniverseById, useUpdateUniverse } from '@/hooks/useUniverses'
import { useCreateMovie, useUpdateMovie, useUnlinkMovie } from '@/hooks/useMovies'
import { universeKeys } from '@/hooks/useUniverses'
import { useQueryClient } from '@tanstack/react-query'
import { UniverseModal } from './UniverseModal'
import { LinkMovieModal } from './LinkMovieModal'
import { MovieModal } from '@/components/movies/MovieModal'
import { ViewCountControls } from '@/components/movies/ViewCountControls'
import { ConfirmDialog } from '@/components/ui/ConfirmDialog'
import { Spinner } from '@/components/ui/Spinner'
import { REWATCH_STATUS_LABELS, REWATCH_STATUS_COLORS } from '@/types/movie'
import type { MovieInUniverse } from '@/types/universe'
import type { CreateMoviePayload } from '@/lib/api/movies'
import type { CreateUniversePayload, UpdateUniversePayload } from '@/lib/api/universes'

interface Props {
  universeId: number
}

export function UniverseDetailPage({ universeId }: Props) {
  const qc = useQueryClient()
  const { data: universe, isPending, isError } = useUniverseById(universeId)
  const updateUniverse = useUpdateUniverse()
  const createMovie = useCreateMovie()
  const updateMovie = useUpdateMovie()
  const unlinkMovie = useUnlinkMovie()

  const [editUniverseOpen, setEditUniverseOpen] = useState(false)
  const [createMovieOpen, setCreateMovieOpen] = useState(false)
  const [linkMovieOpen, setLinkMovieOpen] = useState(false)
  const [editMovie, setEditMovie] = useState<MovieInUniverse | null>(null)
  const [unlinkTarget, setUnlinkTarget] = useState<MovieInUniverse | null>(null)

  const handleUpdateUniverse = async (payload: CreateUniversePayload | UpdateUniversePayload) => {
    await updateUniverse.mutateAsync(payload as UpdateUniversePayload)
    setEditUniverseOpen(false)
  }

  const handleCreateMovie = async (payload: CreateMoviePayload) => {
    await createMovie.mutateAsync({ ...payload, universeId })
    setCreateMovieOpen(false)
    qc.invalidateQueries({ queryKey: universeKeys.detail(universeId) })
  }

  const handleUpdateMovie = async (payload: CreateMoviePayload) => {
    if (!editMovie) return
    await updateMovie.mutateAsync({ id: editMovie.id, payload })
    setEditMovie(null)
    qc.invalidateQueries({ queryKey: universeKeys.detail(universeId) })
  }

  const handleUnlink = async () => {
    if (!unlinkTarget) return
    await unlinkMovie.mutateAsync(unlinkTarget.id)
    setUnlinkTarget(null)
    qc.invalidateQueries({ queryKey: universeKeys.detail(universeId) })
  }

  if (isPending) {
    return (
      <main className="px-6 py-8">
        <div className="flex justify-center py-16"><Spinner /></div>
      </main>
    )
  }

  if (isError || !universe) {
    return (
      <main className="px-6 py-8">
        <Link href="/universes" className="inline-flex items-center gap-1.5 text-sm text-gray-500 hover:text-gray-700 mb-6 transition-colors">
          <ArrowLeft className="w-4 h-4" /> Universes
        </Link>
        <p className="text-center text-rose-500 py-16">Universe not found.</p>
      </main>
    )
  }

  // Build a Movie-compatible object from MovieInUniverse for the edit modal
  const toMovieShape = (m: MovieInUniverse) => ({
    id: m.id,
    title: m.title,
    releaseYear: m.releaseYear,
    durationMinutes: m.durationMinutes,
    reviewText: m.reviewText,
    score: m.score,
    viewCount: m.viewCount,
    rewatchStatus: m.rewatchStatus,
    universeId,
    universeName: universe.name,
    listNo: 0,
    createdAt: '',
    updatedAt: '',
  })

  return (
    <main className="px-6 py-8">
      <Link href="/universes" className="inline-flex items-center gap-1.5 text-sm text-gray-500 hover:text-gray-700 mb-6 transition-colors">
        <ArrowLeft className="w-4 h-4" /> Universes
      </Link>

      {/* Universe header */}
      <div className="bg-white rounded-2xl border border-gray-100 p-6 shadow-sm mb-6">
        <div className="flex items-start justify-between gap-4">
          <div className="flex-1 min-w-0">
            <div className="flex items-center gap-2 mb-1">
              <h1 className="text-xl font-bold text-gray-900">{universe.name}</h1>
              {universe.isHidden && (
                <span className="inline-flex items-center gap-1 px-2 py-0.5 rounded-full text-xs font-medium bg-gray-100 text-gray-500">
                  <EyeOff className="w-3 h-3" /> Hidden
                </span>
              )}
            </div>
            {universe.description && (
              <p className="text-sm text-gray-500 mt-1">{universe.description}</p>
            )}
            <p className="text-xs text-gray-400 mt-2">{universe.movies.length} movie{universe.movies.length !== 1 ? 's' : ''}</p>
          </div>
          <button
            onClick={() => setEditUniverseOpen(true)}
            className="shrink-0 flex items-center gap-2 px-3 py-1.5 rounded-xl text-sm text-gray-600 hover:bg-gray-100 border border-gray-200 transition-colors"
          >
            <svg className="w-3.5 h-3.5" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
            Edit
          </button>
        </div>
      </div>

      {/* Movies section */}
      <div className="flex items-center justify-between mb-4">
        <h2 className="text-base font-semibold text-gray-700">Movies</h2>
        <div className="flex items-center gap-2">
          <button
            onClick={() => setLinkMovieOpen(true)}
            className="flex items-center gap-1.5 px-3 py-1.5 rounded-xl text-sm text-gray-600 hover:bg-gray-100 border border-gray-200 transition-colors"
          >
            <Link2 className="w-3.5 h-3.5" />
            Link existing
          </button>
          <button
            onClick={() => setCreateMovieOpen(true)}
            className="flex items-center gap-1.5 px-3 py-1.5 rounded-xl text-sm font-medium text-white bg-indigo-500 hover:bg-indigo-600 transition-colors"
          >
            <Plus className="w-3.5 h-3.5" />
            New movie
          </button>
        </div>
      </div>

      {universe.movies.length === 0 ? (
        <div className="rounded-2xl border border-gray-100 bg-white p-12 text-center text-gray-400 text-sm">
          No movies yet. Create one or link an existing movie.
        </div>
      ) : (
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
                <th className="px-4 py-3" />
              </tr>
            </thead>
            <tbody className="divide-y divide-gray-50">
              {universe.movies.map(movie => (
                <tr key={movie.id} className="hover:bg-gray-50/50 transition-colors">
                  <td className="px-4 py-3 font-medium text-gray-800 max-w-[200px]">
                    <span className="truncate block">{movie.title}</span>
                  </td>
                  <td className="px-4 py-3 text-gray-500 hidden sm:table-cell">{movie.releaseYear}</td>
                  <td className="px-4 py-3 text-gray-500 hidden md:table-cell">{movie.durationMinutes} min</td>
                  <td className="px-4 py-3 text-gray-700 tabular-nums">
                    {movie.score != null ? Number(movie.score).toFixed(1) : <span className="text-gray-300">—</span>}
                  </td>
                  <td className="px-4 py-3 hidden lg:table-cell">
                    <span className={`inline-flex px-2 py-0.5 rounded-full text-xs font-medium ${REWATCH_STATUS_COLORS[movie.rewatchStatus]}`}>
                      {REWATCH_STATUS_LABELS[movie.rewatchStatus]}
                    </span>
                  </td>
                  <td className="px-4 py-3">
                    <ViewCountControls movieId={movie.id} viewCount={movie.viewCount} />
                  </td>
                  <td className="px-4 py-3">
                    <div className="flex items-center gap-1 justify-end">
                      <ActionBtn onClick={() => setEditMovie(movie)} title="Edit">
                        <svg className="w-4 h-4" fill="none" stroke="currentColor" viewBox="0 0 24 24"><path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z" /></svg>
                      </ActionBtn>
                      <ActionBtn onClick={() => setUnlinkTarget(movie)} title="Unlink from universe" danger>
                        <svg className="w-4 h-4" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth={2}>
                          <path strokeLinecap="round" strokeLinejoin="round" d="M13.828 10.172a4 4 0 00-5.656 0l-4 4a4 4 0 105.656 5.656l1.102-1.101m6.758-6.758a4 4 0 010 5.656l-4 4a4 4 0 01-5.656-5.656l1.1-1.1M9 9l6 6" />
                        </svg>
                      </ActionBtn>
                    </div>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      )}

      {/* Modals */}
      <UniverseModal
        open={editUniverseOpen}
        universe={universe}
        isPending={updateUniverse.isPending}
        onClose={() => setEditUniverseOpen(false)}
        onSubmit={handleUpdateUniverse}
      />

      <MovieModal
        open={createMovieOpen}
        prefillUniverseId={universeId}
        isPending={createMovie.isPending}
        onClose={() => setCreateMovieOpen(false)}
        onSubmit={handleCreateMovie}
      />

      <MovieModal
        open={!!editMovie}
        movie={editMovie ? toMovieShape(editMovie) : null}
        isPending={updateMovie.isPending}
        onClose={() => setEditMovie(null)}
        onSubmit={handleUpdateMovie}
      />

      <LinkMovieModal
        open={linkMovieOpen}
        universeId={universeId}
        existingMovieIds={universe.movies.map(m => m.id)}
        onClose={() => {
          setLinkMovieOpen(false)
          qc.invalidateQueries({ queryKey: universeKeys.detail(universeId) })
        }}
      />

      <ConfirmDialog
        open={!!unlinkTarget}
        title="Unlink movie?"
        description={`"${unlinkTarget?.title}" will be removed from this universe.`}
        confirmLabel="Unlink"
        cancelLabel="Cancel"
        isDanger
        isPending={unlinkMovie.isPending}
        onConfirm={handleUnlink}
        onCancel={() => setUnlinkTarget(null)}
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
