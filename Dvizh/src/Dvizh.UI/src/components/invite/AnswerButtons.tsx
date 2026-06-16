'use client'

import { useState } from 'react'
import { Loader2 } from 'lucide-react'
import type { MascotImages } from '@/lib/constants'
import type { InviteStrings } from '@/lib/i18n'
import { InviteAnswer } from '@/types/invite'
import { useDelayedPending } from '@/hooks/useDelayedPending'

type HoverState = 'none' | 'yes' | 'no'

interface AnswerButtonsProps {
  images: MascotImages
  strings: InviteStrings
  onHoverChange: (src: string) => void
  onAnswer: (answer: InviteAnswer) => void
  isPending: boolean
}

export function AnswerButtons({ images, strings, onHoverChange, onAnswer, isPending }: AnswerButtonsProps) {
  const [hover, setHover] = useState<HoverState>('none')
  const showSpinner = useDelayedPending(isPending)

  const handleEnter = (state: HoverState) => {
    if (isPending) return
    setHover(state)
    onHoverChange(state === 'yes' ? images.hoverYes : images.hoverNo)
  }

  const handleLeave = () => {
    if (isPending) return
    setHover('none')
    onHoverChange(images.neutral)
  }

  return (
    <div className="flex gap-4 justify-center">
      <button
        onClick={() => onAnswer(InviteAnswer.Yes)}
        onMouseEnter={() => handleEnter('yes')}
        onMouseLeave={handleLeave}
        disabled={isPending}
        className={`
          px-8 py-3 rounded-2xl font-semibold text-white text-lg transition-all duration-200
          ${hover === 'yes' ? 'bg-emerald-500 scale-105 shadow-lg shadow-emerald-200' : 'bg-emerald-400'}
          disabled:opacity-50 disabled:cursor-not-allowed
        `}
      >
        {showSpinner && hover === 'yes'
          ? <Loader2 className="animate-spin inline w-5 h-5" />
          : strings.yes}
      </button>

      <button
        onClick={() => onAnswer(InviteAnswer.No)}
        onMouseEnter={() => handleEnter('no')}
        onMouseLeave={handleLeave}
        disabled={isPending}
        className={`
          px-8 py-3 rounded-2xl font-semibold text-white text-lg transition-all duration-200
          ${hover === 'no' ? 'bg-rose-500 scale-105 shadow-lg shadow-rose-200' : 'bg-rose-400'}
          disabled:opacity-50 disabled:cursor-not-allowed
        `}
      >
        {showSpinner && hover === 'no'
          ? <Loader2 className="animate-spin inline w-5 h-5" />
          : strings.no}
      </button>
    </div>
  )
}
