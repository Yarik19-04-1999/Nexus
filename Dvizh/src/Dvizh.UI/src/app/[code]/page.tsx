'use client'

import { use, useState } from 'react'
import { MascotDisplay } from '@/components/invite/MascotDisplay'
import { InviteCard } from '@/components/invite/InviteCard'
import { AnswerButtons } from '@/components/invite/AnswerButtons'
import { AnsweredState } from '@/components/invite/AnsweredState'
import { useOpenInvite, useRespondToInvite, useResetAndRefetch } from '@/hooks/useInvites'
import { CAT_IMAGES, DUCK_IMAGES } from '@/lib/constants'
import { strings } from '@/lib/strings'
import { InviteAnswer, InviteMascot } from '@/types/invite'

interface Props {
  params: Promise<{ code: string }>
}

export default function InvitePage({ params }: Props) {
  const { code } = use(params)

  const { data: invite, isPending: isLoading, error } = useOpenInvite(code)
  const respond = useRespondToInvite(code)
  const reset = useResetAndRefetch(code)

  const images = invite?.mascot === InviteMascot.UtyaDuck ? DUCK_IMAGES : CAT_IMAGES
  const [mascotSrc, setMascotSrc] = useState<string | null>(null)

  if (isLoading) {
    return <PageShell><div className="w-10 h-10 border-4 border-gray-200 border-t-emerald-400 rounded-full animate-spin" /></PageShell>
  }

  if (error || !invite) {
    return (
      <PageShell>
        <MascotDisplay src={CAT_IMAGES.no} />
        <p className="text-gray-500">{strings.invite.notFound}</p>
      </PageShell>
    )
  }

  const isExpired = invite.expiresAt ? new Date(invite.expiresAt) < new Date() : false
  const isAnswered = invite.answer !== InviteAnswer.Pending

  const mascotForAnswer = invite.answer === InviteAnswer.Yes ? images.yes : images.no
  const activeMascot = isAnswered ? mascotForAnswer : (mascotSrc ?? images.neutral)

  const handleAnswer = (answer: InviteAnswer) => {
    respond.mutate(answer, {
      onSuccess: () => setMascotSrc(images.neutral),
    })
  }

  const handleChangeAnswer = () => {
    reset.mutate(invite.id)
  }

  return (
    <PageShell>
      <InviteCard message={invite.message} description={invite.description}>
        <MascotDisplay src={activeMascot} />

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
            images={images}
            onHoverChange={setMascotSrc}
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
