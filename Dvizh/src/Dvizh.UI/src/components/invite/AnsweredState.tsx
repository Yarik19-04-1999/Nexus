'use client'

import { Loader2 } from 'lucide-react'
import { InviteAnswer } from '@/types/invite'
import type { InviteStrings } from '@/lib/i18n'
import { useDelayedPending } from '@/hooks/useDelayedPending'

interface AnsweredStateProps {
  answer: InviteAnswer.Yes | InviteAnswer.No
  strings: InviteStrings
  onChangeAnswer: () => void
  isPending: boolean
}

export function AnsweredState({ answer, strings, onChangeAnswer, isPending }: AnsweredStateProps) {
  const showSpinner = useDelayedPending(isPending)

  const config = {
    [InviteAnswer.Yes]: { label: strings.answeredYes, color: 'text-emerald-600' },
    [InviteAnswer.No]: { label: strings.answeredNo, color: 'text-rose-500' },
  }

  const { label, color } = config[answer]

  return (
    <div className="flex flex-col items-center gap-6">
      <p className={`text-2xl font-bold ${color}`}>{label}</p>
      <button
        onClick={onChangeAnswer}
        disabled={isPending}
        className="mt-4 text-sm text-gray-400 hover:text-gray-600 underline underline-offset-4 transition-colors disabled:cursor-not-allowed"
      >
        {showSpinner
          ? <Loader2 className="animate-spin inline w-4 h-4 mr-1" />
          : null}
        {strings.changeAnswer}
      </button>
    </div>
  )
}
