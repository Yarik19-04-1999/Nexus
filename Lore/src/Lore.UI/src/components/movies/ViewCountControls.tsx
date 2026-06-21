'use client'

import { useState } from 'react'
import { Minus, Plus } from 'lucide-react'
import { useIncrementViewCount, useDecrementViewCount } from '@/hooks/useMovies'

interface ViewCountControlsProps {
  movieId: number
  viewCount: number
}

export function ViewCountControls({ movieId, viewCount }: ViewCountControlsProps) {
  const [incrementedThisSession, setIncrementedThisSession] = useState(false)
  const [localCount, setLocalCount] = useState(viewCount)
  const increment = useIncrementViewCount()
  const decrement = useDecrementViewCount()

  const handleIncrement = async () => {
    const res = await increment.mutateAsync(movieId)
    setLocalCount(res.viewCount)
    setIncrementedThisSession(true)
  }

  const handleDecrement = async () => {
    const res = await decrement.mutateAsync(movieId)
    setLocalCount(res.viewCount)
    setIncrementedThisSession(false)
  }

  const busy = increment.isPending || decrement.isPending

  return (
    <div className="flex items-center gap-1">
      {incrementedThisSession && (
        <button
          onClick={handleDecrement}
          disabled={busy}
          className="w-6 h-6 flex items-center justify-center rounded-lg text-gray-400 hover:text-rose-500 hover:bg-rose-50 transition-colors disabled:opacity-30"
        >
          <Minus className="w-3 h-3" />
        </button>
      )}
      <span className="min-w-[2rem] text-center text-sm text-gray-700 tabular-nums">{localCount}</span>
      <button
        onClick={handleIncrement}
        disabled={busy}
        className="w-6 h-6 flex items-center justify-center rounded-lg text-gray-400 hover:text-indigo-500 hover:bg-indigo-50 transition-colors disabled:opacity-30"
      >
        <Plus className="w-3 h-3" />
      </button>
    </div>
  )
}
