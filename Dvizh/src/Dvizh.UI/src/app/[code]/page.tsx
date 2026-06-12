'use client'

import { use, useState } from 'react'
import { CatDisplay } from '@/components/invite/CatDisplay'
import { InviteCard } from '@/components/invite/InviteCard'
import { AnswerButtons } from '@/components/invite/AnswerButtons'
import { AnsweredState } from '@/components/invite/AnsweredState'
import { useOpenInvite, useRespondToInvite, useResetAndRefetch } from '@/hooks/useInvites'
import { CAT_IMAGES } from '@/lib/constants'
import { strings } from '@/lib/strings'
import { InviteAnswer } from '@/types/invite'

interface Props {
  params: Promise<{ code: string }>
}

export default function InvitePage({ params }: Props) {
  const { code } = use(params)
  const [catSrc, setCatSrc] = useState(CAT_IMAGES.neutral)

  const { data: invite, isPending: isLoading, error } = useOpenInvite(code)
  const respond = useRespondToInvite(code)
  const reset = useResetAndRefetch(code)

  if (isLoading) {
    return <PageShell><div className="w-10 h-10 border-4 border-gray-200 border-t-emerald-400 rounded-full animate-spin" /></PageShell>
  }

  if (error || !invite) {
    return (
      <PageShell>
        <CatDisplay src={CAT_IMAGES.no} />
        <p className="text-gray-500">{strings.invite.notFound}</p>
      </PageShell>
    )
  }

  const isExpired = invite.expiresAt ? new Date(invite.expiresAt) < new Date() : false
  const isAnswered = invite.answer !== InviteAnswer.Pending

  const catForAnswer = invite.answer === InviteAnswer.Yes ? CAT_IMAGES.yes : CAT_IMAGES.no
  const activeCat = isAnswered ? catForAnswer : catSrc

  const handleAnswer = (answer: InviteAnswer) => {
    respond.mutate(answer, {
      onSuccess: () => setCatSrc(CAT_IMAGES.neutral),
    })
  }

  const handleChangeAnswer = () => {
    reset.mutate(invite.id)
  }

  return (
    <PageShell>
      <InviteCard message={invite.message} description={invite.description}>
        <CatDisplay src={activeCat} />

        {isExpired && !isAnswered ? (
          <p className="text-gray-400">{strings.invite.expired}</p>
        ) : isAnswered ? (
          <AnsweredState
            answer={invite.answer as InviteAnswer.Yes | InviteAnswer.No}
            onChangeAnswer={handleChangeAnswer}
            isPending={reset.isPending}
          />
        ) : (
          <AnswerButtons
            onHoverChange={setCatSrc}
            onAnswer={handleAnswer}
            isPending={respond.isPending}
          />
        )}
      </InviteCard>
    </PageShell>
  )
}

function PageShell({ children }: { children: React.ReactNode }) {
  return (
    <main className="min-h-screen flex items-center justify-center bg-white p-6">
      <div className="w-full max-w-sm">{children}</div>
    </main>
  )
}
