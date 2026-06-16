'use client'

import { useForm } from 'react-hook-form'
import { zodResolver } from '@hookform/resolvers/zod'
import { z } from 'zod'
import { useRouter } from 'next/navigation'
import { useAdminStrings } from './AdminLanguageContext'
import { useDelayedPending } from '@/hooks/useDelayedPending'
import { Spinner } from '@/components/ui/Spinner'
import { InviteLanguage, InviteMascot, type Invite } from '@/types/invite'
import type { CreateInvitePayload, UpdateInvitePayload } from '@/lib/api/invites'

const schema = z.object({
  message: z.string().min(1).max(200),
  description: z.string().max(200).optional(),
  expiresAt: z.string().optional(),
  language: z.nativeEnum(InviteLanguage),
  mascot: z.nativeEnum(InviteMascot),
})

type FormValues = z.infer<typeof schema>

interface InviteFormProps {
  invite?: Invite
  onSubmit: (payload: CreateInvitePayload | UpdateInvitePayload) => Promise<void>
  isPending: boolean
}

export function InviteForm({ invite, onSubmit, isPending }: InviteFormProps) {
  const router = useRouter()
  const showSpinner = useDelayedPending(isPending)
  const { strings } = useAdminStrings()
  const s = strings.admin.form

  const { register, handleSubmit, watch, setValue, formState: { errors } } = useForm<FormValues>({
    resolver: zodResolver(schema),
    defaultValues: {
      message: invite?.message ?? '',
      description: invite?.description ?? '',
      expiresAt: invite?.expiresAt ? invite.expiresAt.slice(0, 16) : '',
      language: invite?.language ?? InviteLanguage.Russian,
      mascot: invite?.mascot ?? InviteMascot.MochiPeachCat,
    },
  })

  const selectedMascot = watch('mascot')

  const submit = handleSubmit(async (values) => {
    const payload = invite
      ? { id: invite.id, ...values, expiresAt: values.expiresAt || undefined }
      : { ...values, expiresAt: values.expiresAt || undefined }
    await onSubmit(payload)
    router.push('/admin')
  })

  return (
    <form onSubmit={submit} className="flex flex-col gap-5">
      <Field label={s.message} error={errors.message?.message}>
        <textarea
          {...register('message')}
          rows={3}
          placeholder={s.messagePlaceholder}
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm resize-none focus:outline-none focus:ring-2 focus:ring-emerald-300"
        />
      </Field>

      <Field label={s.description} error={errors.description?.message}>
        <textarea
          {...register('description')}
          rows={2}
          placeholder={s.descriptionPlaceholder}
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm resize-none focus:outline-none focus:ring-2 focus:ring-emerald-300"
        />
      </Field>

      <Field label={s.expiresAt} error={errors.expiresAt?.message}>
        <input
          type="datetime-local"
          {...register('expiresAt')}
          onKeyDown={(e) => e.preventDefault()}
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-emerald-300 cursor-pointer"
        />
      </Field>

      <Field label={s.language} error={errors.language?.message}>
        <select
          {...register('language', { valueAsNumber: true })}
          className="w-full rounded-xl border border-gray-200 px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-emerald-300 bg-white"
        >
          {(Object.values(InviteLanguage).filter((v) => typeof v === 'number') as InviteLanguage[]).map((lang) => (
            <option key={lang} value={lang}>
              {s.languages[lang]}
            </option>
          ))}
        </select>
      </Field>

      <Field label={s.mascot} error={errors.mascot?.message}>
        <div className="flex gap-3">
          {(Object.values(InviteMascot).filter((v) => typeof v === 'number') as InviteMascot[]).map((m) => (
            <button
              key={m}
              type="button"
              onClick={() => setValue('mascot', m)}
              className={`flex-1 flex flex-col items-center gap-1 py-3 px-4 rounded-xl border-2 transition-all
                ${selectedMascot === m
                  ? 'border-emerald-400 bg-emerald-50 text-emerald-700'
                  : 'border-gray-200 bg-white text-gray-500 hover:border-gray-300'}`}
            >
              <span className="text-2xl">{m === InviteMascot.MochiPeachCat ? '🐱' : '🦆'}</span>
              <span className="text-xs font-medium">{s.mascots[m]}</span>
            </button>
          ))}
        </div>
      </Field>

      <div className="flex gap-3 justify-end pt-2">
        <button
          type="button"
          onClick={() => router.push('/admin')}
          className="px-5 py-2 rounded-xl text-sm text-gray-500 hover:bg-gray-100 transition-colors"
        >
          {s.cancel}
        </button>
        <button
          type="submit"
          disabled={isPending}
          className="px-5 py-2 rounded-xl text-sm font-medium text-white bg-emerald-500 hover:bg-emerald-600 transition-colors disabled:opacity-50 flex items-center gap-2"
        >
          {showSpinner && <Spinner className="w-4 h-4 border-white border-t-transparent" />}
          {s.save}
        </button>
      </div>
    </form>
  )
}

function Field({ label, error, children }: { label: string; error?: string; children: React.ReactNode }) {
  return (
    <div className="flex flex-col gap-1">
      <label className="text-sm font-medium text-gray-700">{label}</label>
      {children}
      {error && <p className="text-xs text-rose-500">{error}</p>}
    </div>
  )
}
