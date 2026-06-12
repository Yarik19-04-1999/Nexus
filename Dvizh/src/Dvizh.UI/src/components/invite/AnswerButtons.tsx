'use client'

import { useState } from 'react'
import { Loader2 } from 'lucide-react'
import { CAT_IMAGES } from '@/lib/constants'
import { strings } from '@/lib/strings'
import { InviteAnswer } from '@/types/invite'
import { useDelayedPending } from '@/hooks/useDelayedPending'

type HoverState = 'none' | 'yes' | 'no'

interface AnswerButtonsProps {
  onHoverChange: (src: string) => void
  onAnswer: (answer: InviteAnswer) => void
  isPending: boolean
}

export function AnswerButtons({ onHoverChange, onAnswer, isPending }: AnswerButtonsProps) {
  const [hover, setHover] = useState<HoverState>('none')
  const showSpinner = useDelayedPending(isPending)

  const handleEnter = (state: HoverState) => {
    if (isPending) return
    setHover(state)
    onHoverChange(state === 'yes' ? CAT_IMAGES.hoverYes : CAT_IMAGES.hoverNo)
  }

  const handleLeave = () => {
    if (isPending) return
    setHover('none')
    onHoverChange(CAT_IMAGES.neutral)
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
          : strings.invite.yes}
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
          : strings.invite.no}
      </button>
    </div>
  )
}
