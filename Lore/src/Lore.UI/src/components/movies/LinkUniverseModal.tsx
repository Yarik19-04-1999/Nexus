'use client'

import { useState, useEffect, useRef } from 'react'
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react'
import { X, Search, Loader2 } from 'lucide-react'
import { useSearchUniverses } from '@/hooks/useUniverses'
import { useLinkMovie, useUnlinkMovie } from '@/hooks/useMovies'
import type { Movie } from '@/types/movie'

interface LinkUniverseModalProps {
  movie: Movie | null
  onClose: () => void
}

export function LinkUniverseModal({ movie, onClose }: LinkUniverseModalProps) {
  const [q, setQ] = useState('')
  const [debouncedQ, setDebouncedQ] = useState('')
  const inputRef = useRef<HTMLInputElement>(null)

  const link = useLinkMovie()
  const unlink = useUnlinkMovie()

  useEffect(() => {
    if (movie) {
      setQ('')
      setDebouncedQ('')
      setTimeout(() => inputRef.current?.focus(), 50)
    }
  }, [movie])

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedQ(q), 200)
    return () => clearTimeout(timer)
  }, [q])

  const { data: results, isFetching } = useSearchUniverses(debouncedQ)

  const handleLink = async (universeId: number) => {
    if (!movie) return
    await link.mutateAsync({ movieId: movie.id, universeId })
    onClose()
  }

  const handleUnlink = async () => {
    if (!movie) return
    await unlink.mutateAsync(movie.id)
    onClose()
  }

  const busy = link.isPending || unlink.isPending

  return (
    <Dialog open={!!movie} onClose={onClose} className="relative z-50">
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4">
        <DialogPanel className="w-full max-w-md rounded-2xl bg-white p-6 shadow-xl">
          <div className="flex items-center justify-between mb-4">
            <DialogTitle className="text-base font-semibold text-gray-900">
              Link to universe
            </DialogTitle>
            <button onClick={onClose} className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors">
              <X className="w-4 h-4" />
            </button>
          </div>

          {movie?.universeName && (
            <div className="mb-4 p-3 rounded-xl bg-indigo-50 border border-indigo-100 flex items-center justify-between">
              <div>
                <p className="text-xs text-indigo-500 font-medium mb-0.5">Currently linked</p>
                <p className="text-sm text-indigo-800 font-medium">{movie.universeName}</p>
              </div>
              <button
                onClick={handleUnlink}
                disabled={busy}
                className="text-xs text-rose-500 hover:text-rose-700 font-medium disabled:opacity-50 transition-colors"
              >
                Unlink
              </button>
            </div>
          )}

          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400" />
            {isFetching && <Loader2 className="absolute right-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400 animate-spin" />}
            <input
              ref={inputRef}
              type="text"
              value={q}
              onChange={e => setQ(e.target.value)}
              placeholder="Search universes (min 3 chars)…"
              className="w-full pl-9 pr-9 py-2 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
            />
          </div>

          {debouncedQ.length >= 3 && (
            <div className="mt-2 max-h-60 overflow-y-auto rounded-xl border border-gray-100 divide-y divide-gray-50">
              {!results?.length ? (
                <p className="text-sm text-gray-400 text-center py-6">No universes found</p>
              ) : (
                results.map(u => (
                  <button
                    key={u.id}
                    onClick={() => handleLink(u.id)}
                    disabled={busy || u.id === movie?.universeId}
                    className="w-full text-left px-4 py-3 text-sm hover:bg-indigo-50 transition-colors disabled:opacity-40 disabled:cursor-not-allowed"
                  >
                    <span className="font-medium text-gray-800">{u.name}</span>
                    {u.id === movie?.universeId && (
                      <span className="ml-2 text-xs text-indigo-400">current</span>
                    )}
                  </button>
                ))
              )}
            </div>
          )}

          {debouncedQ.length > 0 && debouncedQ.length < 3 && (
            <p className="mt-2 text-xs text-gray-400 text-center">Type at least 3 characters to search</p>
          )}
        </DialogPanel>
      </div>
    </Dialog>
  )
}
