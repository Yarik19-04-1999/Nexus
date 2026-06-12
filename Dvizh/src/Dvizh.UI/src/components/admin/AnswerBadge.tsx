import { InviteAnswer } from '@/types/invite'
import { strings } from '@/lib/strings'

const styles: Record<InviteAnswer, string> = {
  [InviteAnswer.Pending]: 'bg-gray-100 text-gray-500',
  [InviteAnswer.Yes]: 'bg-emerald-100 text-emerald-700',
  [InviteAnswer.No]: 'bg-rose-100 text-rose-600',
}

export function AnswerBadge({ answer }: { answer: InviteAnswer }) {
  return (
    <span className={`px-2.5 py-0.5 rounded-full text-xs font-medium ${styles[answer]}`}>
      {strings.admin.answers[answer]}
    </span>
  )
}
