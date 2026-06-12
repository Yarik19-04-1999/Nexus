'use client'

import { Loader2 } from 'lucide-react'
import { InviteAnswer } from '@/types/invite'
import { strings } from '@/lib/strings'
import { useDelayedPending } from '@/hooks/useDelayedPending'

interface AnsweredStateProps {
  answer: InviteAnswer.Yes | InviteAnswer.No
  onChangeAnswer: () => void
  isPending: boolean
}

const config = {
  [InviteAnswer.Yes]: {
    label: strings.invite.answeredYes,
    color: 'text-emerald-600',
  },
  [InviteAnswer.No]: {
    label: strings.invite.answeredNo,
    color: 'text-rose-500',
  },
}

export function AnsweredState({ answer, onChangeAnswer, isPending }: AnsweredStateProps) {
  const showSpinner = useDelayedPending(isPending)
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
        {strings.invite.changeAnswer}
      </button>
    </div>
  )
}
