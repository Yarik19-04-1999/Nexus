'use client'

import { useState, useEffect, useRef } from 'react'
import { Dialog, DialogPanel, DialogTitle } from '@headlessui/react'
import { X, Search, Loader2 } from 'lucide-react'
import { useSearchMovies, useLinkMovie } from '@/hooks/useMovies'

interface LinkMovieModalProps {
  open: boolean
  universeId: number
  existingMovieIds: number[]
  onClose: () => void
}

export function LinkMovieModal({ open, universeId, existingMovieIds, onClose }: LinkMovieModalProps) {
  const [q, setQ] = useState('')
  const [debouncedQ, setDebouncedQ] = useState('')
  const inputRef = useRef<HTMLInputElement>(null)
  const link = useLinkMovie()

  useEffect(() => {
    if (open) {
      setQ('')
      setDebouncedQ('')
      setTimeout(() => inputRef.current?.focus(), 50)
    }
  }, [open])

  useEffect(() => {
    const timer = setTimeout(() => setDebouncedQ(q), 200)
    return () => clearTimeout(timer)
  }, [q])

  const { data: rawResults, isFetching } = useSearchMovies(debouncedQ)

  // exclude movies already in this universe
  const results = rawResults?.filter(m => !existingMovieIds.includes(m.id))

  const handleLink = async (movieId: number) => {
    await link.mutateAsync({ movieId, universeId })
    onClose()
  }

  return (
    <Dialog open={open} onClose={onClose} className="relative z-50">
      <div className="fixed inset-0 bg-black/30" aria-hidden="true" />
      <div className="fixed inset-0 flex items-center justify-center p-4">
        <DialogPanel className="w-full max-w-md rounded-2xl bg-white p-6 shadow-xl">
          <div className="flex items-center justify-between mb-4">
            <DialogTitle className="text-base font-semibold text-gray-900">
              Link existing movie
            </DialogTitle>
            <button onClick={onClose} className="p-1.5 rounded-lg text-gray-400 hover:text-gray-600 hover:bg-gray-100 transition-colors">
              <X className="w-4 h-4" />
            </button>
          </div>

          <div className="relative">
            <Search className="absolute left-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400" />
            {isFetching && <Loader2 className="absolute right-3 top-1/2 -translate-y-1/2 w-3.5 h-3.5 text-gray-400 animate-spin" />}
            <input
              ref={inputRef}
              type="text"
              value={q}
              onChange={e => setQ(e.target.value)}
              placeholder="Search movies by title (min 3 chars)…"
              className="w-full pl-9 pr-9 py-2 rounded-xl border border-gray-200 text-sm focus:outline-none focus:ring-2 focus:ring-indigo-300"
            />
          </div>

          {debouncedQ.length >= 3 && (
            <div className="mt-2 max-h-64 overflow-y-auto rounded-xl border border-gray-100 divide-y divide-gray-50">
              {!results?.length ? (
                <p className="text-sm text-gray-400 text-center py-6">
                  {rawResults?.length ? 'All matching movies are already in this universe' : 'No movies found'}
                </p>
              ) : (
                results.map(m => (
                  <button
                    key={m.id}
                    onClick={() => handleLink(m.id)}
                    disabled={link.isPending}
                    className="w-full text-left px-4 py-3 hover:bg-indigo-50 transition-colors disabled:opacity-50"
                  >
                    <span className="text-sm font-medium text-gray-800">{m.title}</span>
                    <span className="ml-2 text-xs text-gray-400">{m.releaseYear}</span>
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
